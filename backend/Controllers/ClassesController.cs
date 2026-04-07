using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly IClassService _service;

    public ClassesController(IClassService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassResponseDto>>> Get([FromQuery] int schoolId)
    {
        if (schoolId <= 0) return BadRequest("SchoolId is required");
        return Ok(await _service.GetClassesAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassResponseDto>> GetById(int id)
    {
        var @class = await _service.GetClassByIdAsync(id);
        if (@class == null) return NotFound();
        return Ok(@class);
    }

    [HttpPost]
    public async Task<ActionResult<ClassResponseDto>> Create(ClassCreateDto dto)
    {
        var created = await _service.CreateClassAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClassResponseDto>> Update(int id, ClassUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateClassAsync(id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _service.DeleteClassAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
