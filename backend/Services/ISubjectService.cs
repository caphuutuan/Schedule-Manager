namespace ScheduleManager.Services;
using ScheduleManager.DTOs;

public interface ISubjectService
{
    Task<IEnumerable<SubjectResponseDto>> GetSubjectsAsync(int schoolId);
    Task<SubjectResponseDto?> GetSubjectByIdAsync(int schoolId, int id);
    Task<SubjectResponseDto> CreateSubjectAsync(SubjectCreateDto dto);
    Task<SubjectResponseDto> UpdateSubjectAsync(int schoolId, int id, SubjectUpdateDto dto);
    Task<bool> DeleteSubjectAsync(int schoolId, int id);
}
