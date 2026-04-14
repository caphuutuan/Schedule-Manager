using ScheduleManager.DTOs;
using ScheduleManager.Models;

namespace ScheduleManager.Services;

public interface IAcademicYearService
{
    Task<IEnumerable<AcademicYearResponseDto>> GetAllAsync(int schoolId);
    Task<AcademicYearResponseDto?> GetActiveAsync(int schoolId);
    Task<AcademicYearResponseDto> CreateAsync(int schoolId, AcademicYearCreateDto dto);

    Task<AcademicYear> GetAcademicYearForDateAsync(int schoolId, DateTime date);
    int GetWeekNumber(DateTime date, DateTime startDate);
    int GetSemester(int weekNumber);
}
