namespace ScheduleManager.DTOs;

public class SubjectResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int SchoolId { get; set; }
}

public class SubjectCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int SchoolId { get; set; }
}

public class SubjectUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}
