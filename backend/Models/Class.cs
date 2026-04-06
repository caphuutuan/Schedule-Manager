namespace ScheduleManager.Models;

public class Class
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    /// <summary>Grade level, e.g. 10, 11, 12</summary>
    public int Grade { get; set; }
    public int SchoolId { get; set; }

    // Navigation properties
    public School School { get; set; } = null!;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
