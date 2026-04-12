using ScheduleManager.DTOs;

namespace ScheduleManager.Services;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleResponseDto>> GetSchedulesAsync(int schoolId, ScheduleFilterDto filter);
    Task<ScheduleResponseDto?> GetScheduleByIdAsync(int schoolId, int id);
    Task<ScheduleResponseDto> CreateScheduleAsync(ScheduleCreateDto dto);
    Task<ScheduleResponseDto> UpdateScheduleAsync(int schoolId, int id, ScheduleUpdateDto dto);
    Task DeleteScheduleAsync(int schoolId, int id);
}
