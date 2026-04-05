namespace ScheduleManager.Repositories;
using ScheduleManager.Data;
using ScheduleManager.Models;
using Microsoft.EntityFrameworkCore;

public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
{
    public SubjectRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Subject>> GetBySchoolIdAsync(int schoolId)
    {
        return await dbSet
            .Include(s => s.Department)
            .Where(s => s.SchoolId == schoolId)
            .ToListAsync();
    }
}
