namespace ScheduleManager.Repositories;
using ScheduleManager.Data;
using ScheduleManager.Models;
using Microsoft.EntityFrameworkCore;

public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
{
    public DepartmentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Department>> GetBySchoolIdAsync(int schoolId)
    {
        return await dbSet
            .Where(d => d.SchoolId == schoolId)
            .ToListAsync();
    }
}
