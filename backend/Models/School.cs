namespace ScheduleManager.Models;

public class School
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation properties
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Class> Classes { get; set; } = new List<Class>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
