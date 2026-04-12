using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentsController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentResponseDto>>> Get(int schoolId)
    {
        return Ok(await _service.GetDepartmentsAsync(schoolId));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DepartmentResponseDto>> GetById(int schoolId, int id)
    {
        var department = await _service.GetDepartmentByIdAsync(schoolId, id);
        if (department == null) return NotFound();
        return Ok(department);
    }

    [HttpPost]
    public async Task<ActionResult<DepartmentResponseDto>> Create(int schoolId, DepartmentCreateDto dto)
    {
        dto.SchoolId = schoolId;
        var created = await _service.CreateDepartmentAsync(dto);
        return CreatedAtAction(nameof(GetById), new { schoolId = schoolId, id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DepartmentResponseDto>> Update(int schoolId, int id, DepartmentUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateDepartmentAsync(schoolId, id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int schoolId, int id)
    {
        var success = await _service.DeleteDepartmentAsync(schoolId, id);
        if (!success) return NotFound();
        return NoContent();
    }
}
