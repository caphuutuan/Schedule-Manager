using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectsController(ISubjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectResponseDto>>> Get([FromQuery] int schoolId)
    {
        if (schoolId <= 0) return BadRequest("SchoolId is required");
        return Ok(await _service.GetSubjectsAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectResponseDto>> GetById(int id)
    {
        var subject = await _service.GetSubjectByIdAsync(id);
        if (subject == null) return NotFound();
        return Ok(subject);
    }

    [HttpPost]
    public async Task<ActionResult<SubjectResponseDto>> Create(SubjectCreateDto dto)
    {
        var created = await _service.CreateSubjectAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubjectResponseDto>> Update(int id, SubjectUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateSubjectAsync(id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _service.DeleteSubjectAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
