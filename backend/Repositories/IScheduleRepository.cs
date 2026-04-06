using ScheduleManager.Models;
using ScheduleManager.DTOs;

namespace ScheduleManager.Repositories;

public interface IScheduleRepository : IGenericRepository<Schedule>
{
    Task<IEnumerable<Schedule>> GetFilteredSchedulesAsync(ScheduleFilterDto filter);
    Task<bool> HasConflictAsync(int exceptionScheduleId, int teacherId, int classId, int dayOfWeek, int period);
}
