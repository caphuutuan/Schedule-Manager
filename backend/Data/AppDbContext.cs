using Microsoft.EntityFrameworkCore;
using ScheduleManager.Models;

namespace ScheduleManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<School> Schools => Set<School>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Schedule> Schedules => Set<Schedule>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── School ──────────────────────────────────────────────────────
        modelBuilder.Entity<School>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
        });

        // ─── AcademicYear ────────────────────────────────────────────────
        modelBuilder.Entity<AcademicYear>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.School)
             .WithMany(s => s.AcademicYears)
             .HasForeignKey(x => x.SchoolId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Department ──────────────────────────────────────────────────
        modelBuilder.Entity<Department>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.HasOne(x => x.School)
             .WithMany(s => s.Departments)
             .HasForeignKey(x => x.SchoolId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Teacher ─────────────────────────────────────────────────────
        modelBuilder.Entity<Teacher>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.HasOne(x => x.Department)
             .WithMany(d => d.Teachers)
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.School)
             .WithMany(s => s.Teachers)
             .HasForeignKey(x => x.SchoolId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Class ───────────────────────────────────────────────────────
        modelBuilder.Entity<Class>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(100);
            e.HasOne(x => x.School)
             .WithMany(s => s.Classes)
             .HasForeignKey(x => x.SchoolId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Subject ─────────────────────────────────────────────────────
        modelBuilder.Entity<Subject>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Name).IsRequired().HasMaxLength(200);
            e.HasOne(x => x.Department)
             .WithMany(d => d.Subjects)
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.School)
             .WithMany()
             .HasForeignKey(x => x.SchoolId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Schedule ────────────────────────────────────────────────────
        modelBuilder.Entity<Schedule>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasOne(x => x.Subject)
             .WithMany(s => s.Schedules)
             .HasForeignKey(x => x.SubjectId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.School)
             .WithMany(s => s.Schedules)
             .HasForeignKey(x => x.SchoolId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Class)
             .WithMany(c => c.Schedules)
             .HasForeignKey(x => x.ClassId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Teacher)
             .WithMany(t => t.Schedules)
             .HasForeignKey(x => x.TeacherId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.AcademicYear)
             .WithMany(y => y.Schedules)
             .HasForeignKey(x => x.AcademicYearId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Seed data ───────────────────────────────────────────────────
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // School
        modelBuilder.Entity<School>().HasData(
            new School { Id = 1, Name = "Trường THPT Nguyễn Du", Level = SchoolLevel.High }
        );

        // Academic Year
        var academicYearStartDate = new DateTime(2025, 9, 5);
        modelBuilder.Entity<AcademicYear>().HasData(
            new AcademicYear 
            { 
                Id = 1, 
                SchoolId = 1, 
                Name = "2025-2026", 
                StartDate = academicYearStartDate, 
                EndDate = new DateTime(2026, 5, 25), 
                IsActive = true 
            }
        );

        // Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Tổ Toán - Lý - Tin", SchoolId = 1 },
            new Department { Id = 2, Name = "Tổ Văn - Sử - Địa", SchoolId = 1 }
        );

        // Teachers
        modelBuilder.Entity<Teacher>().HasData(
            new Teacher { Id = 1, Name = "Nguyễn Văn An",   DepartmentId = 1, SchoolId = 1 },
            new Teacher { Id = 2, Name = "Trần Thị Bình",   DepartmentId = 1, SchoolId = 1 },
            new Teacher { Id = 3, Name = "Lê Văn Cường",    DepartmentId = 1, SchoolId = 1 },
            new Teacher { Id = 4, Name = "Phạm Thị Dung",   DepartmentId = 2, SchoolId = 1 },
            new Teacher { Id = 5, Name = "Hoàng Văn Ê",     DepartmentId = 2, SchoolId = 1 }
        );

        // Classes
        modelBuilder.Entity<Class>().HasData(
            new Class { Id = 1, Name = "10A1", Grade = 10, SchoolId = 1 },
            new Class { Id = 2, Name = "11B2", Grade = 11, SchoolId = 1 },
            new Class { Id = 3, Name = "12C3", Grade = 12, SchoolId = 1 }
        );

        // Subjects
        modelBuilder.Entity<Subject>().HasData(
            new Subject { Id = 1, Name = "Toán", DepartmentId = 1, SchoolId = 1 },
            new Subject { Id = 2, Name = "Vật Lý", DepartmentId = 1, SchoolId = 1 },
            new Subject { Id = 3, Name = "Tin Học", DepartmentId = 1, SchoolId = 1 },
            new Subject { Id = 4, Name = "Ngữ Văn", DepartmentId = 2, SchoolId = 1 },
            new Subject { Id = 5, Name = "Lịch Sử", DepartmentId = 2, SchoolId = 1 },
            new Subject { Id = 6, Name = "Địa Lý", DepartmentId = 2, SchoolId = 1 }
        );

        // Schedules — Using dates in 2025 to match the AcademicYear
        var w1Mon = new DateTime(2025, 9, 8);
        var w1Tue = new DateTime(2025, 9, 9);
        var w1Wed = new DateTime(2025, 9, 10);
        var w1Thu = new DateTime(2025, 9, 11);
        var w1Fri = new DateTime(2025, 9, 12);
        var weekNum = ((w1Mon - academicYearStartDate).Days / 7) + 1;

        modelBuilder.Entity<Schedule>().HasData(
            new Schedule { Id = 1,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 1, TeacherId = 1, SubjectId = 1, DayOfWeek = 1, Period = 1, Date = w1Mon },
            new Schedule { Id = 2,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 1, TeacherId = 2, SubjectId = 2, DayOfWeek = 1, Period = 2, Date = w1Mon },
            new Schedule { Id = 3,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 2, TeacherId = 3, SubjectId = 3, DayOfWeek = 2, Period = 1, Date = w1Tue },
            new Schedule { Id = 4,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 2, TeacherId = 4, SubjectId = 4, DayOfWeek = 2, Period = 2, Date = w1Tue },
            new Schedule { Id = 5,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 3, TeacherId = 5, SubjectId = 5, DayOfWeek = 3, Period = 1, Date = w1Wed },
            new Schedule { Id = 6,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 1, TeacherId = 1, SubjectId = 1, DayOfWeek = 3, Period = 2, Date = w1Wed },
            new Schedule { Id = 7,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 2, TeacherId = 2, SubjectId = 2, DayOfWeek = 4, Period = 1, Date = w1Thu },
            new Schedule { Id = 8,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 3, TeacherId = 3, SubjectId = 3, DayOfWeek = 4, Period = 2, Date = w1Thu },
            new Schedule { Id = 9,  SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 1, TeacherId = 4, SubjectId = 4, DayOfWeek = 5, Period = 1, Date = w1Fri },
            new Schedule { Id = 10, SchoolId = 1, AcademicYearId = 1, WeekNumber = weekNum, ClassId = 2, TeacherId = 5, SubjectId = 6, DayOfWeek = 5, Period = 2, Date = w1Fri }
        );
    }
}
