namespace ScheduleManager.Services;
using ScheduleManager.DTOs;

public interface ISubjectService
{
    Task<IEnumerable<SubjectResponseDto>> GetSubjectsAsync(int schoolId);
    Task<SubjectResponseDto?> GetSubjectByIdAsync(int id);
    Task<SubjectResponseDto> CreateSubjectAsync(SubjectCreateDto dto);
    Task<SubjectResponseDto> UpdateSubjectAsync(int id, SubjectUpdateDto dto);
    Task<bool> DeleteSubjectAsync(int id);
}
