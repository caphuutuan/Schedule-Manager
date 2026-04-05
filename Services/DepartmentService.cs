namespace ScheduleManager.Services;
using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;
    private readonly IMapper _mapper;

    public DepartmentService(IDepartmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentResponseDto>> GetDepartmentsAsync(int schoolId)
    {
        var departments = await _repository.GetBySchoolIdAsync(schoolId);
        return _mapper.Map<IEnumerable<DepartmentResponseDto>>(departments);
    }

    public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department == null) return null;
        return _mapper.Map<DepartmentResponseDto>(department);
    }

    public async Task<DepartmentResponseDto> CreateDepartmentAsync(DepartmentCreateDto dto)
    {
        var department = _mapper.Map<Department>(dto);
        await _repository.AddAsync(department);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DepartmentResponseDto>(department);
    }

    public async Task<DepartmentResponseDto> UpdateDepartmentAsync(int id, DepartmentUpdateDto dto)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department == null) throw new KeyNotFoundException("Không tìm thấy tổ chuyên môn");

        _mapper.Map(dto, department);
        _repository.Update(department);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DepartmentResponseDto>(department);
    }

    public async Task<bool> DeleteDepartmentAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department == null) return false;

        _repository.Remove(department);
        await _repository.SaveChangesAsync();
        return true;
    }
}
