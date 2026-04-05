namespace ScheduleManager.Models;

public class Teacher
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int SchoolId { get; set; }

    // Navigation properties
    public Department Department { get; set; } = null!;
    public School School { get; set; } = null!;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
