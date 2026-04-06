namespace ScheduleManager.Models;

public class Schedule
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public int ClassId { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }

    /// <summary>
    /// Day of week: 1 = Monday, 2 = Tuesday, ..., 7 = Sunday
    /// Stores the recurring day so schedules can repeat weekly.
    /// </summary>
    public int DayOfWeek { get; set; }

    /// <summary>
    /// Represents the specific class period (e.g. 1, 2, 3...)
    /// </summary>
    public int Period { get; set; }

    // Navigation properties
    public School School { get; set; } = null!;
    public Class Class { get; set; } = null!;
    public Teacher Teacher { get; set; } = null!;
    public Subject Subject { get; set; } = null!;
}
