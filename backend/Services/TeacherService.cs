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

    public async Task<TeacherResponseDto?> GetTeacherByIdAsync(int id)
    {
        var teacher = await _repository.GetDetailsByIdAsync(id);
        if (teacher == null) return null;
        return _mapper.Map<TeacherResponseDto>(teacher);
    }

    public async Task<TeacherResponseDto> CreateTeacherAsync(TeacherCreateDto dto)
    {
        var teacher = _mapper.Map<Teacher>(dto);
        await _repository.AddAsync(teacher);
        await _repository.SaveChangesAsync();
        
        teacher = await _repository.GetDetailsByIdAsync(teacher.Id);
        return _mapper.Map<TeacherResponseDto>(teacher!);
    }

    public async Task<TeacherResponseDto> UpdateTeacherAsync(int id, TeacherUpdateDto dto)
    {
        var teacher = await _repository.GetByIdAsync(id);
        if (teacher == null) throw new KeyNotFoundException("Không tìm thấy giáo viên");

        _mapper.Map(dto, teacher);
        _repository.Update(teacher);
        await _repository.SaveChangesAsync();
        
        teacher = await _repository.GetDetailsByIdAsync(teacher.Id);
        return _mapper.Map<TeacherResponseDto>(teacher!);
    }

    public async Task<bool> DeleteTeacherAsync(int id)
    {
        var teacher = await _repository.GetByIdAsync(id);
        if (teacher == null) return false;

        _repository.Remove(teacher);
        await _repository.SaveChangesAsync();
        return true;
    }
}
