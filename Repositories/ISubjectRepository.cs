namespace ScheduleManager.Repositories;
using ScheduleManager.Models;

public interface ISubjectRepository : IGenericRepository<Subject>
{
    Task<IEnumerable<Subject>> GetBySchoolIdAsync(int schoolId);
}
