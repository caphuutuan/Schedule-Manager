using ScheduleManager.DTOs;

namespace ScheduleManager.Services;

public interface IScheduleService
{
    Task<IEnumerable<ScheduleResponseDto>> GetSchedulesAsync(ScheduleFilterDto filter);
    Task<ScheduleResponseDto?> GetScheduleByIdAsync(int id);
    Task<ScheduleResponseDto> CreateScheduleAsync(ScheduleCreateDto dto);
    Task<ScheduleResponseDto> UpdateScheduleAsync(int id, ScheduleUpdateDto dto);
    Task DeleteScheduleAsync(int id);
}
