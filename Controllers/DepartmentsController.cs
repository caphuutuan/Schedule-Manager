using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentsController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> Get([FromQuery] int schoolId)
    {
        if (schoolId <= 0) return BadRequest("SchoolId is required");
        return Ok(await _service.GetDepartmentsAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentResponseDto>> GetById(int id)
    {
        var department = await _service.GetDepartmentByIdAsync(id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponseDto>> Create(DepartmentCreateDto dto)
    {
        var created = await _service.CreateDepartmentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentResponseDto>> Update(int id, DepartmentUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateDepartmentAsync(id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await _service.DeleteDepartmentAsync(id);
        if (!success) return NotFound();
        return NoContent();
    }
}
