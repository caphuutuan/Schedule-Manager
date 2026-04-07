namespace ScheduleManager.Repositories;
using ScheduleManager.Models;

public interface IClassRepository : IGenericRepository<Class>
{
    Task<IEnumerable<Class>> GetBySchoolIdAsync(int schoolId);
}
