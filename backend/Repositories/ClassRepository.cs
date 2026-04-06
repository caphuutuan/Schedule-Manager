namespace ScheduleManager.Repositories;
using ScheduleManager.Data;
using ScheduleManager.Models;
using Microsoft.EntityFrameworkCore;

public class ClassRepository : GenericRepository<Class>, IClassRepository
{
    public ClassRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Class>> GetBySchoolIdAsync(int schoolId)
    {
        return await dbSet
            .Where(c => c.SchoolId == schoolId)
            .ToListAsync();
    }
}
