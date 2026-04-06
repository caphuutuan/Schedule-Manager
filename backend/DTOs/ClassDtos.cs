namespace ScheduleManager.DTOs;

public class ClassResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Grade { get; set; }
    public int SchoolId { get; set; }
}

public class ClassCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int Grade { get; set; }
    public int SchoolId { get; set; }
}

public class ClassUpdateDto
{
    public string Name { get; set; } = string.Empty;
    public int Grade { get; set; }
}
