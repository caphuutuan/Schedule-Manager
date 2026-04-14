namespace ScheduleManager.Models;

public class AcademicYear
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    // ─── Navigation properties ────────────────────────────────────────────────
    public School School { get; set; } = null!;
    public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
