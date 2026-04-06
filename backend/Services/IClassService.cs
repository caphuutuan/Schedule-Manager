namespace ScheduleManager.Services;
using ScheduleManager.DTOs;

public interface IClassService
{
    Task<IEnumerable<ClassResponseDto>> GetClassesAsync(int schoolId);
    Task<ClassResponseDto?> GetClassByIdAsync(int id);
    Task<ClassResponseDto> CreateClassAsync(ClassCreateDto dto);
    Task<ClassResponseDto> UpdateClassAsync(int id, ClassUpdateDto dto);
    Task<bool> DeleteClassAsync(int id);
}
