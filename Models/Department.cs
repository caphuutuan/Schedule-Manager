namespace ScheduleManager.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }

    // Navigation properties
    public School School { get; set; } = null!;
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
