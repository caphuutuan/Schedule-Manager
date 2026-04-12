namespace ScheduleManager.Services;
using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

public class TeacherService : ITeacherService
{
    private readonly ITeacherRepository _repository;
    private readonly IMapper _mapper;

    public TeacherService(ITeacherRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeacherResponseDto>> GetTeachersAsync(int schoolId)
    {
        var teachers = await _repository.GetBySchoolIdAsync(schoolId);
        return _mapper.Map<IEnumerable<TeacherResponseDto>>(teachers);
    }

    public async Task<TeacherResponseDto?> GetTeacherByIdAsync(int schoolId, int id)
    {
        var teacher = await _repository.GetByIdAsync(id);
        if (teacher == null || teacher.SchoolId != schoolId) return null;
        return _mapper.Map<TeacherResponseDto>(teacher);
    }

    public async Task<TeacherResponseDto> CreateTeacherAsync(TeacherCreateDto dto)
    {
        var teacher = _mapper.Map<Teacher>(dto);
        await _repository.AddAsync(teacher);
        await _repository.SaveChangesAsync();
        return _mapper.Map<TeacherResponseDto>(teacher);
    }

    public async Task<TeacherResponseDto> UpdateTeacherAsync(int schoolId, int id, TeacherUpdateDto dto)
    {
        var teacher = await _repository.GetByIdAsync(id);
        if (teacher == null || teacher.SchoolId != schoolId) 
            throw new KeyNotFoundException("Không tìm thấy giáo viên trong trường này");

        _mapper.Map(dto, teacher);
        _repository.Update(teacher);
        await _repository.SaveChangesAsync();
        return _mapper.Map<TeacherResponseDto>(teacher);
    }

    public async Task<bool> DeleteTeacherAsync(int schoolId, int id)
    {
        var teacher = await _repository.GetByIdAsync(id);
        if (teacher == null || teacher.SchoolId != schoolId) return false;

        _repository.Remove(teacher);
        await _repository.SaveChangesAsync();
        return true;
    }
}
