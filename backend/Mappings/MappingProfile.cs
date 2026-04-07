using AutoMapper;
using ScheduleManager.DTOs;
using ScheduleManager.Models;

namespace ScheduleManager.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ─── Schedule ────────────────────────────────────────────────────
        CreateMap<Schedule, ScheduleResponseDto>()
            .ForMember(dest => dest.ClassName,    opt => opt.MapFrom(src => src.Class.Name))
            .ForMember(dest => dest.TeacherName,  opt => opt.MapFrom(src => src.Teacher.Name))
            .ForMember(dest => dest.SubjectName,  opt => opt.MapFrom(src => src.Subject.Name));

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
