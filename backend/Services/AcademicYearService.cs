using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;
using ScheduleManager.Repositories;

namespace ScheduleManager.Services;

public class AcademicYearService : IAcademicYearService
{
    private const int MaxWeeks = 35;
    private const int Semester1EndWeek = 18;

    private readonly IAcademicYearRepository _repository;
    private readonly IMapper _mapper;

    public AcademicYearService(IAcademicYearRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AcademicYearResponseDto>> GetAllAsync(int schoolId)
    {
        var years = await _repository.GetAllBySchoolAsync(schoolId);
        return years.Select(MapToDto);
    }

    public async Task<AcademicYearResponseDto?> GetActiveAsync(int schoolId)
    {
        var year = await _repository.GetActiveAsync(schoolId);
        return year == null ? null : MapToDto(year);
    }

    public async Task<AcademicYearResponseDto> CreateAsync(int schoolId, AcademicYearCreateDto dto)
    {
        if (dto.StartDate >= dto.EndDate)
            throw new ArgumentException("StartDate must be before EndDate.");

        if (dto.IsActive)
        {
            var existingYears = await _repository.GetAllBySchoolAsync(schoolId);
            foreach (var existing in existingYears.Where(y => y.IsActive))
            {
                existing.IsActive = false;
                _repository.Update(existing);
            }
        }

        var academicYear = new AcademicYear
        {
            SchoolId  = schoolId,
            Name      = dto.Name,
            StartDate = dto.StartDate.Date,
            EndDate   = dto.EndDate.Date,
            IsActive  = dto.IsActive
        };

        await _repository.AddAsync(academicYear);
        await _repository.SaveChangesAsync();

        return MapToDto(academicYear);
    }

    public async Task<AcademicYear> GetAcademicYearForDateAsync(int schoolId, DateTime date)
    {
        var year = await _repository.GetByDateAsync(schoolId, date.Date);
        if (year == null)
            throw new KeyNotFoundException("Date is outside academic year.");

        return year;
    }

    public int GetWeekNumber(DateTime date, DateTime startDate)
    {
        int dayOffset = (date.Date - startDate.Date).Days;
        int weekNumber = (dayOffset / 7) + 1;

        if (weekNumber < 1 || weekNumber > MaxWeeks)
            throw new ArgumentOutOfRangeException(
                nameof(weekNumber),
                $"Invalid week number (must be 1–{MaxWeeks}). Calculated: {weekNumber}.");

        return weekNumber;
    }

    public int GetSemester(int weekNumber)
        => weekNumber <= Semester1EndWeek ? 1 : 2;

    private AcademicYearResponseDto MapToDto(AcademicYear year)
    {
        var dto = _mapper.Map<AcademicYearResponseDto>(year);
        int total = ((year.EndDate - year.StartDate).Days / 7) + 1;
        dto.TotalWeeks = Math.Min(total, MaxWeeks);
        return dto;
    }
}
