using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;

namespace ScheduleManager.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ─── AcademicYear ────────────────────────────────────────────────
        CreateMap<AcademicYear, AcademicYearResponseDto>();
        // Note: TotalWeeks is manually computed in the service.

        // ─── Schedule ────────────────────────────────────────────────────
        CreateMap<Schedule, ScheduleResponseDto>()
            .ForMember(dest => dest.ClassName,    opt => opt.MapFrom(src => src.Class.Name))
            .ForMember(dest => dest.TeacherName,  opt => opt.MapFrom(src => src.Teacher.Name))
            .ForMember(dest => dest.SubjectName,  opt => opt.MapFrom(src => src.Subject.Name))
            .ForMember(dest => dest.Semester,     opt => opt.MapFrom(src => src.WeekNumber <= 18 ? 1 : 2));

        // ─── Subject ─────────────────────────────────────────────────────
        CreateMap<Subject, SubjectResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
        CreateMap<SubjectCreateDto, Subject>();
        CreateMap<SubjectUpdateDto, Subject>();

        // ─── Department ──────────────────────────────────────────────────
        CreateMap<Department, DepartmentResponseDto>();
        CreateMap<DepartmentCreateDto, Department>();
        CreateMap<DepartmentUpdateDto, Department>();

        // ─── Teacher ─────────────────────────────────────────────────────
        CreateMap<Teacher, TeacherResponseDto>()
            .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));
        CreateMap<TeacherCreateDto, Teacher>();
        CreateMap<TeacherUpdateDto, Teacher>();

        // ─── Class ───────────────────────────────────────────────────────
        CreateMap<Class, ClassResponseDto>();
        CreateMap<ClassCreateDto, Class>();
        CreateMap<ClassUpdateDto, Class>();

        // ─── School ──────────────────────────────────────────────────────
        CreateMap<School, SchoolResponseDto>();
        CreateMap<SchoolCreateDto, School>();
        CreateMap<SchoolUpdateDto, School>();
    }
}
