using ScheduleManager.Models;

namespace ScheduleManager.Repositories;

public interface IAcademicYearRepository : IGenericRepository<AcademicYear>
{
    Task<AcademicYear?> GetActiveAsync(int schoolId);
    Task<AcademicYear?> GetByDateAsync(int schoolId, DateTime date);
    Task<IEnumerable<AcademicYear>> GetAllBySchoolAsync(int schoolId);
}
