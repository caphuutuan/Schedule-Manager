using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/schools/{schoolId}/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _service;

    public SchedulesController(IScheduleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleResponseDto>>> GetList(int schoolId, [FromQuery] ScheduleFilterDto filter)
    {
        return Ok(await _service.GetSchedulesAsync(schoolId, filter));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleResponseDto>> GetById(int schoolId, int id)
    {
        var schedule = await _service.GetScheduleByIdAsync(schoolId, id);
        if (schedule == null) return NotFound();
        return Ok(schedule);
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleResponseDto>> Create(int schoolId, ScheduleCreateDto dto)
    {
        try
        {
            dto.SchoolId = schoolId;
            var created = await _service.CreateScheduleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { schoolId = schoolId, id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ScheduleResponseDto>> Update(int schoolId, int id, ScheduleUpdateDto dto)
    {
        try
        {
            return Ok(await _service.UpdateScheduleAsync(schoolId, id, dto));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int schoolId, int id)
    {
        try
        {
            await _service.DeleteScheduleAsync(schoolId, id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
