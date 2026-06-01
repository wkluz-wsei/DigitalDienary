using CoreApp.Application.Security;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;
using CoreApp.Domain.ValueObjects;
using Infrastructure.EntityFramework.Entities;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Context;

public class UniversityDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public const string DatabaseFileName = "university.db";

    public static readonly Guid StudentAdamId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f");
    public static readonly Guid StudentEwaId = Guid.Parse("7ba19ea5-3967-4b24-bb87-b14c9ee14770");
    public static readonly Guid CourseAlgorithmsId = Guid.Parse("8e8f8f66-bb84-4f77-8a42-6052b8c64410");
    public static readonly Guid CourseDatabasesId = Guid.Parse("1bcf84f4-57fa-4fb1-a655-bd8ee4d061d2");
    public static readonly Guid LecturerNowakId = Guid.Parse("0d7c4923-0ed0-4db9-a392-4b3c520ef77e");
    public static readonly Guid LecturerKowalskaId = Guid.Parse("05f64714-3ac1-4327-8e02-fd519844126f");
    public static readonly Guid AcademicYear2024Id = Guid.Parse("3bb4d12d-a507-46db-b676-f8d25d73cf59");
    public static readonly Guid AcademicYear2025Id = Guid.Parse("8e7a123e-b9b1-42d5-8bdd-c3cd7d7e96fb");
    public static readonly Guid GradeSeedId = Guid.Parse("f063f470-e91d-447a-96ae-132286d4ca77");

    public const string AdminUserId = "0f73f8f9-0800-40dd-bf77-540069454fe8";
    public const string StudentUserId = "ef444766-304f-4f77-bf64-baf2b23974ae";
    public const string AdminRoleId = "7e79ebd0-aaaf-4888-bc21-3404b1a1818d";
    public const string DeanOfficeStaffRoleId = "f4f4fbb7-8ef2-47fe-8558-79206ad2fbd8";
    public const string LecturerRoleId = "73480eab-37e1-4f7f-ac39-b9e63313ace9";
    public const string StudentRoleId = "97d1d11b-76cc-4320-8bf3-ee52ca1cf0d7";
    public const string AdminRoleConcurrencyStamp = "9aae6557-4608-422c-8081-20c1d7703e04";
    public const string DeanOfficeStaffRoleConcurrencyStamp = "4aef5cef-5339-4e20-a3df-4de38cd4af9f";
    public const string LecturerRoleConcurrencyStamp = "1b0820de-73cd-46a6-9381-d95a02ab5be0";
    public const string StudentRoleConcurrencyStamp = "2aedf543-0be5-4d99-b269-5fa77a62c6f6";
    public const string AdminPasswordHash = "AQAAAAIAAYagAAAAEA0RjJEUakY8kIZ+ldE1OU8lyvxba4g4XTpisBuOdVceGP2hn/UqwTkQtDcdDyuEgw==";
    public const string StudentPasswordHash = "AQAAAAIAAYagAAAAEKEsVWWEm2JWiwk1zHO7Vv84hmXhLrP2mRJEA0VIqqT+d8ENvH0Ojj5MaWINX8J76g==";

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Grade> Grades => Set<Grade>();
    public DbSet<Lecturer> Lecturers => Set<Lecturer>();
    public DbSet<AcademicYear> AcademicYears => Set<AcademicYear>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public UniversityDbContext()
    {
    }

    public UniversityDbContext(DbContextOptions<UniversityDbContext> options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var databasePath = Path.Combine(Directory.GetCurrentDirectory(), DatabaseFileName);
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>(entity =>
        {
            entity.Property(u => u.FirstName).HasMaxLength(100);
            entity.Property(u => u.LastName).HasMaxLength(100);
            entity.Property(u => u.FullName).HasMaxLength(200);
            entity.Property(u => u.Department).HasMaxLength(100);
            entity.Property(u => u.Status).HasConversion<string>();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        builder.Entity<AppRole>(entity =>
        {
            entity.Property(r => r.Name).HasMaxLength(50);
            entity.Property(r => r.Description).HasMaxLength(200);
        });

        builder.Entity<Student>(entity =>
        {
            entity.Property(s => s.FirstName).HasMaxLength(100);
            entity.Property(s => s.LastName).HasMaxLength(100);
            entity.Property(s => s.NationalId)
                .HasConversion(
                    v => v != null ? v.Value : null,
                    v => v != null ? new PESEL(v) : null)
                .HasMaxLength(11);
            entity.Property(s => s.Email).HasMaxLength(200);
            entity.Property(s => s.StudentId).HasMaxLength(50);
            entity.Property(s => s.ProgramName).HasMaxLength(200);
            entity.Property(s => s.Status).HasConversion<string>();
        });

        builder.Entity<Course>(entity =>
        {
            entity.Property(c => c.Code).HasMaxLength(20);
            entity.Property(c => c.Name).HasMaxLength(200);
            entity.Property(c => c.CompletionType).HasConversion<string>();
            entity.Property(c => c.Semester).HasConversion<string>();
            entity.Ignore(c => c.Enrollments);
        });

        builder.Entity<Lecturer>(entity =>
        {
            entity.Property(l => l.FirstName).HasMaxLength(100);
            entity.Property(l => l.LastName).HasMaxLength(100);
            entity.Property(l => l.NationalId)
                .HasConversion(
                    v => v != null ? v.Value : null,
                    v => v != null ? new PESEL(v) : null)
                .HasMaxLength(11);
            entity.Property(l => l.Email).HasMaxLength(200);
            entity.Property(l => l.Title).HasMaxLength(50);
            entity.Property(l => l.Faculty).HasMaxLength(100);
            entity.Ignore(l => l.TaughtCourses);
        });

        builder.Entity<AcademicYear>(entity =>
        {
            entity.Property(y => y.Name).HasMaxLength(20);
        });

        builder.Entity<Grade>(entity =>
        {
            entity.Property(g => g.Date).HasColumnType("TEXT");
            entity.Property(g => g.GradeType).HasConversion<string>();
            entity.Property(g => g.GradeValue).HasConversion<string>();
            entity.Property<Guid>("StudentRefId");
            entity.Property<Guid>("CourseId");
            entity.Property<Guid>("AcademicYearId");
            entity.Property<Guid?>("InstructorId");

            entity.HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey("StudentRefId")
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(g => g.Course)
                .WithMany()
                .HasForeignKey("CourseId")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.AcademicYear)
                .WithMany()
                .HasForeignKey("AcademicYearId")
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(g => g.Instructor)
                .WithMany()
                .HasForeignKey("InstructorId")
                .OnDelete(DeleteBehavior.Restrict);
        });

        SeedIdentity(builder);
        SeedDomain(builder);
    }

    private static void SeedIdentity(ModelBuilder builder)
    {
        builder.Entity<AppRole>().HasData(
            new AppRole(UserRole.Administrator.ToString(), "System administrator")
            {
                Id = AdminRoleId,
                ConcurrencyStamp = AdminRoleConcurrencyStamp,
                NormalizedName = UserRole.Administrator.ToString().ToUpperInvariant()
            },
            new AppRole(UserRole.DeanOfficeStaff.ToString(), "Dean office staff member")
            {
                Id = DeanOfficeStaffRoleId,
                ConcurrencyStamp = DeanOfficeStaffRoleConcurrencyStamp,
                NormalizedName = UserRole.DeanOfficeStaff.ToString().ToUpperInvariant()
            },
            new AppRole(UserRole.Lecturer.ToString(), "University lecturer")
            {
                Id = LecturerRoleId,
                ConcurrencyStamp = LecturerRoleConcurrencyStamp,
                NormalizedName = UserRole.Lecturer.ToString().ToUpperInvariant()
            },
            new AppRole(UserRole.Student.ToString(), "University student")
            {
                Id = StudentRoleId,
                ConcurrencyStamp = StudentRoleConcurrencyStamp,
                NormalizedName = UserRole.Student.ToString().ToUpperInvariant()
            });

        var adminUser = new AppUser
        {
            Id = AdminUserId,
            UserName = "admin@wsei.edu.pl",
            NormalizedUserName = "ADMIN@WSEI.EDU.PL",
            Email = "admin@wsei.edu.pl",
            NormalizedEmail = "ADMIN@WSEI.EDU.PL",
            EmailConfirmed = true,
            FirstName = "Anna",
            LastName = "Admin",
            FullName = "Anna Admin",
            Department = "Dean Office",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 1, 10, 8, 0, 0, DateTimeKind.Utc),
            PasswordHash = AdminPasswordHash,
            SecurityStamp = "c7595157-b163-429e-a80e-3a914904f2b2",
            ConcurrencyStamp = "41193c7e-72d9-44c8-b553-c00d3fb7a1df"
        };

        var studentUser = new AppUser
        {
            Id = StudentUserId,
            UserName = "adam.nowak@wsei.edu.pl",
            NormalizedUserName = "ADAM.NOWAK@WSEI.EDU.PL",
            Email = "adam.nowak@wsei.edu.pl",
            NormalizedEmail = "ADAM.NOWAK@WSEI.EDU.PL",
            EmailConfirmed = true,
            FirstName = "Adam",
            LastName = "Nowak",
            FullName = "Adam Nowak",
            Department = "Students",
            Status = SystemUserStatus.Active,
            CreatedAt = new DateTime(2026, 1, 11, 8, 0, 0, DateTimeKind.Utc),
            PasswordHash = StudentPasswordHash,
            SecurityStamp = "e85bb4bf-37c5-4964-a85e-0a37b7b828ad",
            ConcurrencyStamp = "d4febf57-6ea9-4130-8740-32945d585b1d"
        };

        builder.Entity<AppUser>().HasData(adminUser, studentUser);

        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = AdminUserId, RoleId = AdminRoleId },
            new IdentityUserRole<string> { UserId = AdminUserId, RoleId = DeanOfficeStaffRoleId },
            new IdentityUserRole<string> { UserId = StudentUserId, RoleId = StudentRoleId });
    }

    private static void SeedDomain(ModelBuilder builder)
    {
        builder.Entity<Student>().HasData(
            new Student
            {
                Id = StudentAdamId,
                FirstName = "Adam",
                LastName = "Nowak",
                NationalId = new PESEL("99010112342"),
                Email = "adam.nowak@example.com",
                StudentId = "S001",
                ProgramName = "Informatyka",
                YearOfStudy = 1,
                Status = StudentStatus.Active
            },
            new Student
            {
                Id = StudentEwaId,
                FirstName = "Ewa",
                LastName = "Kowalska",
                NationalId = new PESEL("98020254323"),
                Email = "ewa.kowalska@example.com",
                StudentId = "S002",
                ProgramName = "Matematyka",
                YearOfStudy = 2,
                Status = StudentStatus.OnLeave
            });

        builder.Entity<Course>().HasData(
            new Course
            {
                Id = CourseAlgorithmsId,
                Code = "ALG101",
                Name = "Algorytmy i struktury danych",
                EctsCredits = 6,
                CompletionType = CompletionType.Exam,
                Semester = Semester.Winter
            },
            new Course
            {
                Id = CourseDatabasesId,
                Code = "DBS201",
                Name = "Bazy danych",
                EctsCredits = 5,
                CompletionType = CompletionType.CreditWithGrade,
                Semester = Semester.Summer
            });

        builder.Entity<AcademicYear>().HasData(
            new AcademicYear
            {
                Id = AcademicYear2024Id,
                Name = "2024/2025"
            },
            new AcademicYear
            {
                Id = AcademicYear2025Id,
                Name = "2025/2026"
            });

        builder.Entity<Lecturer>().HasData(
            new Lecturer
            {
                Id = LecturerNowakId,
                FirstName = "Jan",
                LastName = "Nowak",
                NationalId = new PESEL("75010112346"),
                Email = "jan.nowak@wsei.edu.pl",
                Title = "dr inż.",
                Faculty = "Informatyka"
            },
            new Lecturer
            {
                Id = LecturerKowalskaId,
                FirstName = "Anna",
                LastName = "Kowalska",
                NationalId = new PESEL("78020254325"),
                Email = "anna.kowalska@wsei.edu.pl",
                Title = "mgr",
                Faculty = "Matematyka"
            });

        builder.Entity<Grade>().HasData(new
        {
            Id = GradeSeedId,
            Date = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc),
            GradeType = GradeType.Partial,
            GradeValue = GradeValue.Grade40,
            StudentRefId = StudentAdamId,
            CourseId = CourseAlgorithmsId,
            AcademicYearId = AcademicYear2025Id,
            InstructorId = LecturerNowakId
        });
    }
}
