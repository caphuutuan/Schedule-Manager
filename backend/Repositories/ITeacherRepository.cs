namespace ScheduleManager.Repositories;
using ScheduleManager.Models;

public interface ITeacherRepository : IGenericRepository<Teacher>
{
    Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId);
    Task<Teacher?> GetDetailsByIdAsync(int id);
}
