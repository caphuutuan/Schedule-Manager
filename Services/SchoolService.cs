namespace ScheduleManager.Services;
using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

public class SchoolService : ISchoolService
{
    private readonly ISchoolRepository _repository;
    private readonly IMapper _mapper;

    public SchoolService(ISchoolRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SchoolResponseDto>> GetSchoolsAsync()
    {
        var schools = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<SchoolResponseDto>>(schools);
    }

    public async Task<SchoolResponseDto?> GetSchoolByIdAsync(int id)
    {
        var school = await _repository.GetByIdAsync(id);
        if (school == null) return null;
        return _mapper.Map<SchoolResponseDto>(school);
    }

    public async Task<SchoolResponseDto> CreateSchoolAsync(SchoolCreateDto dto)
    {
        var school = _mapper.Map<School>(dto);
        await _repository.AddAsync(school);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SchoolResponseDto>(school);
    }

    public async Task<SchoolResponseDto> UpdateSchoolAsync(int id, SchoolUpdateDto dto)
    {
        var school = await _repository.GetByIdAsync(id);
        if (school == null) throw new KeyNotFoundException("Không tìm thấy trường hc");

        _mapper.Map(dto, school);
        _repository.Update(school);
        await _repository.SaveChangesAsync();
        return _mapper.Map<SchoolResponseDto>(school);
    }

    public async Task<bool> DeleteSchoolAsync(int id)
    {
        var school = await _repository.GetByIdAsync(id);
        if (school == null) return false;

        _repository.Remove(school);
        await _repository.SaveChangesAsync();
        return true;
    }
}
