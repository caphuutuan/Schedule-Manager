using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

namespace ScheduleManager.Services;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;
    private readonly IAcademicYearService _academicYearService;
    private readonly IMapper _mapper;

    public ScheduleService(
        IScheduleRepository repository, 
        IAcademicYearService academicYearService,
        IMapper mapper)
    {
        _repository = repository;
        _academicYearService = academicYearService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ScheduleResponseDto>> GetSchedulesAsync(int schoolId, ScheduleFilterDto filter)
    {
        filter.SchoolId = schoolId;

        // If a single Date is provided, convert it to a WeekNumber using active academic year.
        if (filter.Date.HasValue && !filter.WeekNumber.HasValue)
        {
            try 
            {
                var academicYear = await _academicYearService.GetAcademicYearForDateAsync(schoolId, filter.Date.Value);
                filter.WeekNumber = _academicYearService.GetWeekNumber(filter.Date.Value, academicYear.StartDate);
            }
            catch (KeyNotFoundException)
            {
                // If the date falls outside any academic year, there are no valid schedules
                return Enumerable.Empty<ScheduleResponseDto>();
            }
            catch (ArgumentOutOfRangeException)
            {
                // If it evaluates to an invalid week number, no valid schedules
                return Enumerable.Empty<ScheduleResponseDto>();
            }
        }

        var schedules = await _repository.GetFilteredSchedulesAsync(filter);
        return _mapper.Map<IEnumerable<ScheduleResponseDto>>(schedules);
    }

    public async Task<ScheduleResponseDto?> GetScheduleByIdAsync(int schoolId, int id)
    {
        var schedule = await _repository.GetByIdAsync(id);
        if (schedule == null || schedule.SchoolId != schoolId) return null;
        
        return _mapper.Map<ScheduleResponseDto>(schedule);
    }

    public async Task<ScheduleResponseDto> CreateScheduleAsync(ScheduleCreateDto dto)
    {
        bool hasConflict = await _repository.HasConflictAsync(0, dto.TeacherId, dto.ClassId, dto.DayOfWeek, dto.Period, dto.Date);
        if (hasConflict)
        {
            throw new Exception("Lịch học bị trùng thời gian cho Lớp hoặc Giáo viên này!");
        }

        var schedule = new Schedule
        {
            SchoolId = dto.SchoolId,
            ClassId = dto.ClassId,
            TeacherId = dto.TeacherId,
            SubjectId = dto.SubjectId,
            DayOfWeek = dto.DayOfWeek,
            Period = dto.Period,
            Date = dto.Date
        };

        if (dto.Date.HasValue)
        {
            var academicYear = await _academicYearService.GetAcademicYearForDateAsync(dto.SchoolId, dto.Date.Value);
            int weekNum = _academicYearService.GetWeekNumber(dto.Date.Value, academicYear.StartDate);
            
            schedule.AcademicYearId = academicYear.Id;
            schedule.WeekNumber = weekNum;
        }

        await _repository.AddAsync(schedule);
        await _repository.SaveChangesAsync();

        return _mapper.Map<ScheduleResponseDto>(schedule);
    }

    public async Task<ScheduleResponseDto> UpdateScheduleAsync(int schoolId, int id, ScheduleUpdateDto dto)
    {
        var schedule = await _repository.GetByIdAsync(id);
        if (schedule == null || schedule.SchoolId != schoolId) 
            throw new KeyNotFoundException("Không tìm thấy lịch học trong trường này.");

        bool hasConflict = await _repository.HasConflictAsync(id, dto.TeacherId, dto.ClassId, dto.DayOfWeek, dto.Period, dto.Date);
        if (hasConflict)
        {
            throw new Exception("Lịch học bị trùng thời gian cho Lớp hoặc Giáo viên này!");
        }

        schedule.ClassId = dto.ClassId;
        schedule.TeacherId = dto.TeacherId;
        schedule.SubjectId = dto.SubjectId;
        schedule.DayOfWeek = dto.DayOfWeek;
        schedule.Period = dto.Period;
        schedule.Date = dto.Date;

        if (dto.Date.HasValue)
        {
            var academicYear = await _academicYearService.GetAcademicYearForDateAsync(schoolId, dto.Date.Value);
            int weekNum = _academicYearService.GetWeekNumber(dto.Date.Value, academicYear.StartDate);
            
            schedule.AcademicYearId = academicYear.Id;
            schedule.WeekNumber = weekNum;
        }
        else
        {
            schedule.AcademicYearId = null;
            schedule.WeekNumber = 0;
        }

        _repository.Update(schedule);
        await _repository.SaveChangesAsync();

        return _mapper.Map<ScheduleResponseDto>(schedule);
    }

    public async Task DeleteScheduleAsync(int schoolId, int id)
    {
        var schedule = await _repository.GetByIdAsync(id);
        if (schedule == null || schedule.SchoolId != schoolId) 
            throw new KeyNotFoundException("Không tìm thấy lịch học trong trường này.");

        _repository.Remove(schedule);
        await _repository.SaveChangesAsync();
    }
}
