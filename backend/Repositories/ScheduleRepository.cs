using Microsoft.EntityFrameworkCore;
using ScheduleManager.Data;
using ScheduleManager.Models;
using ScheduleManager.DTOs;

namespace ScheduleManager.Repositories;

public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Schedule>> GetFilteredSchedulesAsync(ScheduleFilterDto filter)
    {
        var query = dbSet
            .Include(s => s.Class)
            .Include(s => s.Teacher)
            .Include(s => s.Subject)
            .AsQueryable();

        // 1. Mandatory Multi-tenant filter
        query = query.Where(s => s.SchoolId == filter.SchoolId);

        // 2. Type filter (class | teacher | department)
        if (!string.IsNullOrEmpty(filter.Type) && filter.Id.HasValue)
        {
            var type = filter.Type.ToLower();
            if (type == "class")
            {
                query = query.Where(s => s.ClassId == filter.Id.Value);
            }
            else if (type == "teacher")
            {
                query = query.Where(s => s.TeacherId == filter.Id.Value);
            }
            else if (type == "department")
            {
                query = query.Where(s => s.Teacher.DepartmentId == filter.Id.Value);
            }
        }

        // 3. WeekNumber filtering
        if (filter.WeekNumber.HasValue)
        {
            query = query.Where(s => s.WeekNumber == filter.WeekNumber.Value || s.WeekNumber == 0);
        }

        // 4. DayOfWeek filtering 
        // If they provided a specific Date but still want to filter, we already converted it to WeekNumber above.
        // We can additionally filter by DayOfWeek if we want to isolate a specific day of that week:
        if (filter.Date.HasValue)
        {
            int dow = GetMappingDayOfWeek(filter.Date.Value);
            query = query.Where(s => s.DayOfWeek == dow);
        }

        return await query.ToListAsync();
    }

    public async Task<bool> HasConflictAsync(int exceptionScheduleId, int teacherId, int classId, int dayOfWeek, int period, DateTime? date = null)
    {
        // Check for conflicts:
        // 1. Same Teacher or Class
        // 2. Same Period
        // 3. Either:
        //    a. Both are the same specific Date
        //    b. Both are recurring on the same DayOfWeek
        //    c. One is recurring and the other is a specific Date that falls on that DayOfWeek
        
        return await dbSet.AnyAsync(s => 
            s.Id != exceptionScheduleId && 
            s.Period == period &&
            (s.TeacherId == teacherId || s.ClassId == classId) &&
            (
                (s.Date == date) || // Both same date (or both null)
                (s.DayOfWeek == dayOfWeek && (!s.Date.HasValue || !date.HasValue)) || // Recurring vs Recurring or Recurring vs Date
                (s.Date.HasValue && date.HasValue && s.Date.Value.Date == date.Value.Date) // Specific Date vs Specific Date
            )
        );
    }

    private int GetMappingDayOfWeek(DateTime date)
    {
        int dow = (int)date.DayOfWeek;
        return dow == 0 ? 7 : dow; // Map Sunday (0) to 7
    }
}
