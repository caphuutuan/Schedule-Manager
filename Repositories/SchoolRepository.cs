namespace ScheduleManager.Repositories;
using ScheduleManager.Data;
using ScheduleManager.Models;

public class SchoolRepository : GenericRepository<School>, ISchoolRepository
{
    public SchoolRepository(AppDbContext context) : base(context) { }
}
