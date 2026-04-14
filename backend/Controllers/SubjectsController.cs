using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/[controller]")]
public class SubjectsController : ControllerBase
{
    private readonly ISubjectService _service;

    public SubjectsController(ISubjectService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubjectResponseDto>>> Get(int schoolId)
    {
        return Ok(await _service.GetSubjectsAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectResponseDto>> GetById(int schoolId, int id)
    {
        var subject = await _service.GetSubjectByIdAsync(schoolId, id);
        if (subject == null) return NotFound();
        return Ok(subject);
    }

    [HttpPost]
    public async Task<ActionResult<SubjectResponseDto>> Create(int schoolId, SubjectCreateDto dto)
    {
        dto.SchoolId = schoolId;
        var created = await _service.CreateSubjectAsync(dto);
        return CreatedAtAction(nameof(GetById), new { schoolId = schoolId, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SubjectResponseDto>> Update(int schoolId, int id, SubjectUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateSubjectAsync(schoolId, id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int schoolId, int id)
    {
        var success = await _service.DeleteSubjectAsync(schoolId, id);
        if (!success) return NotFound();
        return NoContent();
    }
}
