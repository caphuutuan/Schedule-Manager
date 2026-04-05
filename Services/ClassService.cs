namespace ScheduleManager.Services;
using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

public class ClassService : IClassService
{
    private readonly IClassRepository _repository;
    private readonly IMapper _mapper;

    public ClassService(IClassRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ClassResponseDto>> GetClassesAsync(int schoolId)
    {
        var classes = await _repository.GetBySchoolIdAsync(schoolId);
        return _mapper.Map<IEnumerable<ClassResponseDto>>(classes);
    }

    public async Task<ClassResponseDto?> GetClassByIdAsync(int id)
    {
        var @class = await _repository.GetByIdAsync(id);
        if (@class == null) return null;
        return _mapper.Map<ClassResponseDto>(@class);
    }

    public async Task<ClassResponseDto> CreateClassAsync(ClassCreateDto dto)
    {
        var @class = _mapper.Map<Class>(dto);
        await _repository.AddAsync(@class);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ClassResponseDto>(@class);
    }

    public async Task<ClassResponseDto> UpdateClassAsync(int id, ClassUpdateDto dto)
    {
        var @class = await _repository.GetByIdAsync(id);
        if (@class == null) throw new KeyNotFoundException("Không tìm thấy lớp học");

        _mapper.Map(dto, @class);
        _repository.Update(@class);
        await _repository.SaveChangesAsync();
        return _mapper.Map<ClassResponseDto>(@class);
    }

    public async Task<bool> DeleteClassAsync(int id)
    {
        var @class = await _repository.GetByIdAsync(id);
        if (@class == null) return false;

        _repository.Remove(@class);
        await _repository.SaveChangesAsync();
        return true;
    }
}
