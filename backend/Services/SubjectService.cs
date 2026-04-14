namespace ScheduleManager.Services;
using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

public class SubjectService : ISubjectService
{
    private readonly ISubjectRepository _repository;
    private readonly IMapper _mapper;

    public SubjectService(ISubjectRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SubjectResponseDto>> GetSubjectsAsync(int schoolId)
    {
        var subjects = await _repository.GetBySchoolIdAsync(schoolId);
        return _mapper.Map<IEnumerable<SubjectResponseDto>>(subjects);
    }

    public async Task<SubjectResponseDto?> GetSubjectByIdAsync(int schoolId, int id)
    {
        var subject = await _repository.GetByIdAsync(id);
        if (subject == null || subject.SchoolId != schoolId) return null;
        return _mapper.Map<SubjectResponseDto>(subject);
    }

    public async Task<SubjectResponseDto> CreateSubjectAsync(SubjectCreateDto dto)
    {
        var subject = _mapper.Map<Subject>(dto);
        await _repository.AddAsync(subject);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SubjectResponseDto>(subject);
    }

    public async Task<SubjectResponseDto> UpdateSubjectAsync(int schoolId, int id, SubjectUpdateDto dto)
    {
        var subject = await _repository.GetByIdAsync(id);
        if (subject == null || subject.SchoolId != schoolId) 
            throw new KeyNotFoundException("Không tìm thấy môn học trong trường này");

        _mapper.Map(dto, subject);
        _repository.Update(subject);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SubjectResponseDto>(subject);
    }

    public async Task<bool> DeleteSubjectAsync(int schoolId, int id)
    {
        var subject = await _repository.GetByIdAsync(id);
        if (subject == null || subject.SchoolId != schoolId) return false;

        _repository.Remove(subject);
        await _repository.SaveChangesAsync();
        return true;
    }
}
