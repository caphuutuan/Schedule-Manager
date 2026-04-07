namespace ScheduleManager.Models;


public enum SchoolLevel
{
    Elementary = 1, // Grades 1-5
    Middle = 2,     // Grades 6-9
    High = 3,       // Grades 10-12
    K12 = 4         // Grades 1-12
}

public class School
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public SchoolLevel Level { get; set; } = SchoolLevel.High;

    // Navigation properties
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Class> Classes { get; set; } = new List<Class>();
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
