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

        // 3. Date filtering (mapping literal dates to recurring DayOfWeek)
        if (filter.Date.HasValue)
        {
            int dow = GetMappingDayOfWeek(filter.Date.Value);
            query = query.Where(s => s.DayOfWeek == dow);
        }
        else if (filter.FromDate.HasValue && filter.ToDate.HasValue)
        {
            DateTime start = filter.FromDate.Value;
            DateTime end = filter.ToDate.Value;
            
            TimeSpan diff = end - start;
            if (diff.TotalDays >= 0 && diff.TotalDays < 7)
            {
                 var allowedDows = new List<int>();
                 for(var date = start; date <= end; date = date.AddDays(1))
                 {
                     allowedDows.Add(GetMappingDayOfWeek(date));
                 }
                 query = query.Where(s => allowedDows.Contains(s.DayOfWeek));
            }
        }

        return await query.ToListAsync();
    }

    public async Task<bool> HasConflictAsync(int exceptionScheduleId, int teacherId, int classId, int dayOfWeek, int period)
    {
        return await dbSet.AnyAsync(s => 
            s.Id != exceptionScheduleId && 
            s.DayOfWeek == dayOfWeek &&
            s.Period == period &&
            (s.TeacherId == teacherId || s.ClassId == classId)
        );
    }

    private int GetMappingDayOfWeek(DateTime date)
    {
        int dow = (int)date.DayOfWeek;
        return dow == 0 ? 7 : dow; // Map Sunday (0) to 7
    }
}
