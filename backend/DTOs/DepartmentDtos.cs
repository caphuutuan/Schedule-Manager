namespace ScheduleManager.DTOs;

public class DepartmentResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
}

public class DepartmentCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int SchoolId { get; set; }
}

public class DepartmentUpdateDto
{
    public string Name { get; set; } = string.Empty;
}
