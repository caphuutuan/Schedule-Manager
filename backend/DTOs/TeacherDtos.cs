namespace ScheduleManager.DTOs;

public class TeacherResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public int SchoolId { get; set; }
}

public class TeacherCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int SchoolId { get; set; }
}

public class TeacherUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}
