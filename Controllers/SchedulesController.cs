using Microsoft.AspNetCore.Mvc;
using ScheduleManager.DTOs;
using ScheduleManager.Services;

namespace ScheduleManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public SchedulesController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] ScheduleFilterDto filter)
    {
        if (filter.SchoolId <= 0)
        {
            return BadRequest("SchoolId là bắt buộc.");
        }
        var schedules = await _scheduleService.GetSchedulesAsync(filter);
        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var schedule = await _scheduleService.GetScheduleByIdAsync(id);
        if (schedule == null) return NotFound();
        return Ok(schedule);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ScheduleCreateDto dto)
    {
        try
        {
            var result = await _scheduleService.CreateScheduleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ScheduleUpdateDto dto)
    {
        try
        {
            var result = await _scheduleService.UpdateScheduleAsync(id, dto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _scheduleService.DeleteScheduleAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
