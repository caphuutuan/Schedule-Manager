namespace ScheduleManager.Repositories;
using ScheduleManager.Models;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IEnumerable<Department>> GetBySchoolIdAsync(int schoolId);
}
