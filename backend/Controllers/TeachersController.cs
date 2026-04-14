using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;

    public TeachersController(ITeacherService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherResponseDto>>> Get(int schoolId)
    {
        return Ok(await _service.GetTeachersAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherResponseDto>> GetById(int schoolId, int id)
    {
        var teacher = await _service.GetTeacherByIdAsync(schoolId, id);
        if (teacher == null) return NotFound();
        return Ok(teacher);
    }

    [HttpPost]
    public async Task<ActionResult<TeacherResponseDto>> Create(int schoolId, TeacherCreateDto dto)
    {
        dto.SchoolId = schoolId;
        var created = await _service.CreateTeacherAsync(dto);
        return CreatedAtAction(nameof(GetById), new { schoolId = schoolId, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherResponseDto>> Update(int schoolId, int id, TeacherUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateTeacherAsync(schoolId, id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int schoolId, int id)
    {
        var success = await _service.DeleteTeacherAsync(schoolId, id);
        if (!success) return NotFound();
        return NoContent();
    }
}
