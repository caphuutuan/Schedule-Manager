namespace ScheduleManager.Services;
using ScheduleManager.DTOs;

public interface ISchoolService
{
    Task<IEnumerable<SchoolResponseDto>> GetSchoolsAsync();
    Task<SchoolResponseDto?> GetSchoolByIdAsync(int id);
    Task<SchoolResponseDto> CreateSchoolAsync(SchoolCreateDto dto);
    Task<SchoolResponseDto> UpdateSchoolAsync(int id, SchoolUpdateDto dto);
    Task<bool> DeleteSchoolAsync(int id);
}
