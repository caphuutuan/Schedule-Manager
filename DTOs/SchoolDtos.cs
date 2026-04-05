namespace ScheduleManager.DTOs;

public class SchoolResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class SchoolCreateDto
{
    public string Name { get; set; } = string.Empty;
}

public class SchoolUpdateDto
{
    public string Name { get; set; } = string.Empty;
}
