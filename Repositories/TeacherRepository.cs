namespace ScheduleManager.Repositories;
using ScheduleManager.Data;
using ScheduleManager.Models;
using Microsoft.EntityFrameworkCore;

public class TeacherRepository : GenericRepository<Teacher>, ITeacherRepository
{
    public TeacherRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Teacher>> GetBySchoolIdAsync(int schoolId)
    {
        return await dbSet
            .Include(t => t.Department)
            .Where(t => t.SchoolId == schoolId)
            .ToListAsync();
    }

    public async Task<Teacher?> GetDetailsByIdAsync(int id)
    {
        return await dbSet
            .Include(t => t.Department)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}
