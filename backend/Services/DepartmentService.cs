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

    public async Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int schoolId, int id)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department == null || department.SchoolId != schoolId) return null;
        return _mapper.Map<DepartmentResponseDto>(department);
    }

    public async Task<DepartmentResponseDto> CreateDepartmentAsync(DepartmentCreateDto dto)
    {
        var department = _mapper.Map<Department>(dto);
        await _repository.AddAsync(department);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DepartmentResponseDto>(department);
    }

    public async Task<DepartmentResponseDto> UpdateDepartmentAsync(int schoolId, int id, DepartmentUpdateDto dto)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department == null || department.SchoolId != schoolId) 
            throw new KeyNotFoundException("Không tìm thấy tổ bộ môn trong trường này");

        _mapper.Map(dto, department);
        _repository.Update(department);
        await _repository.SaveChangesAsync();
        return _mapper.Map<DepartmentResponseDto>(department);
    }

    public async Task<bool> DeleteDepartmentAsync(int schoolId, int id)
    {
        var department = await _repository.GetByIdAsync(id);
        if (department == null || department.SchoolId != schoolId) return false;

        _repository.Remove(department);
        await _repository.SaveChangesAsync();
        return true;
    }
}
