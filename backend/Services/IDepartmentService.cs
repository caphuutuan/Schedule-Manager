namespace ScheduleManager.Services;
using ScheduleManager.DTOs;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentResponseDto>> GetDepartmentsAsync(int schoolId);
    Task<DepartmentResponseDto?> GetDepartmentByIdAsync(int id);
    Task<DepartmentResponseDto> CreateDepartmentAsync(DepartmentCreateDto dto);
    Task<DepartmentResponseDto> UpdateDepartmentAsync(int id, DepartmentUpdateDto dto);
    Task<bool> DeleteDepartmentAsync(int id);
}
