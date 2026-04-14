namespace ScheduleManager.DTOs;

// ─── Response DTO ────────────────────────────────────────────────────────────

/// <summary>
/// Returned to the client when querying schedules.
/// Includes denormalized names for convenient display.
/// </summary>
public class ScheduleResponseDto
{
    public int Id { get; set; }
    public int SchoolId { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int TeacherId { get; set; }
    public string TeacherName { get; set; } = string.Empty;
    public int SubjectId { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public int DayOfWeek { get; set; }
    public int Period { get; set; }
    public DateTime? Date { get; set; }
    public int WeekNumber { get; set; }
    public int Semester { get; set; }
    public int? AcademicYearId { get; set; }
}

// ─── Create DTO ──────────────────────────────────────────────────────────────

/// <summary>Payload for POST /api/schedules</summary>
public class ScheduleCreateDto
{
    public int SchoolId { get; set; }
    public int ClassId { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }

    /// <summary>1 = Monday … 7 = Sunday</summary>
    public int DayOfWeek { get; set; }

    /// <summary>The specific class period (e.g., 1, 2, 3)</summary>
    public int Period { get; set; }

    public DateTime? Date { get; set; }
}

// ─── Update DTO ──────────────────────────────────────────────────────────────

/// <summary>Payload for PUT /api/schedules/{id}</summary>
public class ScheduleUpdateDto
{
    public int ClassId { get; set; }
    public int TeacherId { get; set; }
    public int SubjectId { get; set; }
    public int DayOfWeek { get; set; }
    public int Period { get; set; }
    public DateTime? Date { get; set; }
}

// ─── Filter DTO ──────────────────────────────────────────────────────────────

/// <summary>Bound from query-string for GET /api/schedules</summary>
public class ScheduleFilterDto
{
    /// <summary>Required – isolates data per school (multi-tenant)</summary>
    public int SchoolId { get; set; }

    /// <summary>class | teacher | department</summary>
    public string? Type { get; set; }

    /// <summary>Id of the entity identified by Type</summary>
    public int? Id { get; set; }

    /// <summary>Filter by a single calendar date (extracts DayOfWeek)</summary>
    public DateTime? Date { get; set; }

    /// <summary>Start of a date range (inclusive)</summary>
    public DateTime? FromDate { get; set; }

    /// <summary>End of a date range (inclusive)</summary>
    public DateTime? ToDate { get; set; }

    public int? WeekNumber { get; set; }
}
