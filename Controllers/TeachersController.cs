using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _service;

    public TeachersController(ITeacherService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeacherResponseDto>>> Get([FromQuery] int schoolId)
    {
        if (schoolId <= 0) return BadRequest("SchoolId is required");
        return Ok(await _service.GetTeachersAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeacherResponseDto>> GetById(int id)
    {
        var teacher = await _service.GetTeacherByIdAsync(id);
        if (teacher == null) return NotFound();
        return Ok(teacher);
    }

    [HttpPost]
    public async Task<ActionResult<TeacherResponseDto>> Create(TeacherCreateDto dto)
    {
        var created = await _service.CreateTeacherAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TeacherResponseDto>> Update(int id, TeacherUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateTeacherAsync(id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _service.DeleteTeacherAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
