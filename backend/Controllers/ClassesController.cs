using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly IClassService _service;

    public ClassesController(IClassService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassResponseDto>>> Get(int schoolId)
    {
        return Ok(await _service.GetClassesAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClassResponseDto>> GetById(int schoolId, int id)
    {
        var @class = await _service.GetClassByIdAsync(schoolId, id);
        if (@class == null) return NotFound();
        return Ok(@class);
    }

    [HttpPost]
    public async Task<ActionResult<ClassResponseDto>> Create(int schoolId, ClassCreateDto dto)
    {
        dto.SchoolId = schoolId; // Ensure schoolId matches the route
        var created = await _service.CreateClassAsync(dto);
        return CreatedAtAction(nameof(GetById), new { schoolId = schoolId, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClassResponseDto>> Update(int schoolId, int id, ClassUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateClassAsync(schoolId, id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int schoolId, int id)
    {
        var success = await _service.DeleteClassAsync(schoolId, id);
        if (!success) return NotFound();
        return NoContent();
    }
}
