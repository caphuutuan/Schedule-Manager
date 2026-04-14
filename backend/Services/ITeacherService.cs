namespace ScheduleManager.Services;
using ScheduleManager.DTOs;

public interface ITeacherService
{
    Task<IEnumerable<TeacherResponseDto>> GetTeachersAsync(int schoolId);
    Task<TeacherResponseDto?> GetTeacherByIdAsync(int schoolId, int id);
    Task<TeacherResponseDto> CreateTeacherAsync(TeacherCreateDto dto);
    Task<TeacherResponseDto> UpdateTeacherAsync(int schoolId, int id, TeacherUpdateDto dto);
    Task<bool> DeleteTeacherAsync(int schoolId, int id);
}
