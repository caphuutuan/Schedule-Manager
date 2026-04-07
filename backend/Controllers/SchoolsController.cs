using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolsController : ControllerBase
{
    private readonly ISchoolService _service;

    public SchoolsController(ISchoolService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SchoolResponseDto>>> GetAll()
    {
        return Ok(await _service.GetSchoolsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SchoolResponseDto>> GetById(int id)
    {
        var school = await _service.GetSchoolByIdAsync(id);
        if (school == null) return NotFound();
        return Ok(school);
    }

    [HttpPost]
    public async Task<ActionResult<SchoolResponseDto>> Create(SchoolCreateDto dto)
    {
        var created = await _service.CreateSchoolAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SchoolResponseDto>> Update(int id, SchoolUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateSchoolAsync(id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _service.DeleteSchoolAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
