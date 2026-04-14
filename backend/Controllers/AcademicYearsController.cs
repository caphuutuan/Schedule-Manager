using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/academic-years")]
public class AcademicYearsController : ControllerBase
{
    private readonly IAcademicYearService _service;

    public AcademicYearsController(IAcademicYearService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AcademicYearResponseDto>>> GetAll(int schoolId)
    {
        var years = await _service.GetAllAsync(schoolId);
        return Ok(years);
    }

    [HttpGet("active")]
    public async Task<ActionResult<AcademicYearResponseDto>> GetActive(int schoolId)
    {
        var year = await _service.GetActiveAsync(schoolId);
        if (year == null)
            return NotFound(new { message = "Academic year not found." });

        return Ok(year);
    }

    [HttpPost]
    public async Task<ActionResult<AcademicYearResponseDto>> Create(int schoolId, AcademicYearCreateDto dto)
    {
        try
        {
            var created = await _service.CreateAsync(schoolId, dto);
            return CreatedAtAction(nameof(GetActive), new { schoolId }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
