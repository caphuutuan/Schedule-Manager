using Microsoft.EntityFrameworkCore;
using ScheduleManager.Data;
using ScheduleManager.Models;

namespace ScheduleManager.Repositories;

public class AcademicYearRepository : GenericRepository<AcademicYear>, IAcademicYearRepository
{
    public AcademicYearRepository(AppDbContext context) : base(context) { }

    public async Task<AcademicYear?> GetActiveAsync(int schoolId)
        => await dbSet.FirstOrDefaultAsync(a => a.SchoolId == schoolId && a.IsActive);

    public async Task<AcademicYear?> GetByDateAsync(int schoolId, DateTime date)
        => await dbSet.FirstOrDefaultAsync(a =>
            a.SchoolId == schoolId &&
            a.StartDate <= date &&
            a.EndDate   >= date);

    public async Task<IEnumerable<AcademicYear>> GetAllBySchoolAsync(int schoolId)
        => await dbSet
            .Where(a => a.SchoolId == schoolId)
            .OrderByDescending(a => a.StartDate)
            .ToListAsync();
}
