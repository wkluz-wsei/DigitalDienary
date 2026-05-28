using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;
using Infrastructure.EntityFramework.Context;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seeders;

public class UniversityDbSeeder(UniversityDbContext context) : IDataSeeder
{
    public int Order => 2;

    public async Task SeedAsync()
    {
        if (!await context.Students.AnyAsync())
        {
            await context.Students.AddRangeAsync(
                new Student
                {
                    Id = UniversityDbContext.StudentAdamId,
                    FirstName = "Adam",
                    LastName = "Nowak",
                    NationalId = "99010112345",
                    Email = "adam.nowak@example.com",
                    StudentId = "S001",
                    ProgramName = "Informatyka",
                    YearOfStudy = 1,
                    Status = StudentStatus.Active
                },
                new Student
                {
                    Id = UniversityDbContext.StudentEwaId,
                    FirstName = "Ewa",
                    LastName = "Kowalska",
                    NationalId = "98020254321",
                    Email = "ewa.kowalska@example.com",
                    StudentId = "S002",
                    ProgramName = "Matematyka",
                    YearOfStudy = 2,
                    Status = StudentStatus.OnLeave
                });
        }

        if (!await context.Courses.AnyAsync())
        {
            await context.Courses.AddRangeAsync(
                new Course
                {
                    Id = UniversityDbContext.CourseAlgorithmsId,
                    Code = "ALG101",
                    Name = "Algorytmy i struktury danych",
                    EctsCredits = 6,
                    CompletionType = CompletionType.Exam,
                    Semester = Semester.Winter
                },
                new Course
                {
                    Id = UniversityDbContext.CourseDatabasesId,
                    Code = "DBS201",
                    Name = "Bazy danych",
                    EctsCredits = 5,
                    CompletionType = CompletionType.CreditWithGrade,
                    Semester = Semester.Summer
                });
        }

        if (!await context.AcademicYears.AnyAsync())
        {
            await context.AcademicYears.AddRangeAsync(
                new AcademicYear { Id = UniversityDbContext.AcademicYear2024Id, Name = "2024/2025" },
                new AcademicYear { Id = UniversityDbContext.AcademicYear2025Id, Name = "2025/2026" });
        }

        if (!await context.Lecturers.AnyAsync())
        {
            await context.Lecturers.AddRangeAsync(
                new Lecturer
                {
                    Id = UniversityDbContext.LecturerNowakId,
                    FirstName = "Jan",
                    LastName = "Nowak",
                    NationalId = "75010112345",
                    Email = "jan.nowak@wsei.edu.pl",
                    Title = "dr inż.",
                    Faculty = "Informatyka"
                },
                new Lecturer
                {
                    Id = UniversityDbContext.LecturerKowalskaId,
                    FirstName = "Anna",
                    LastName = "Kowalska",
                    NationalId = "78020254321",
                    Email = "anna.kowalska@wsei.edu.pl",
                    Title = "mgr",
                    Faculty = "Matematyka"
                });
        }

        await context.SaveChangesAsync();

        if (!await context.Grades.AnyAsync())
        {
            var student = await context.Students.FirstAsync(s => s.Id == UniversityDbContext.StudentAdamId);
            var course = await context.Courses.FirstAsync(c => c.Id == UniversityDbContext.CourseAlgorithmsId);
            var year = await context.AcademicYears.FirstAsync(y => y.Id == UniversityDbContext.AcademicYear2025Id);
            var lecturer = await context.Lecturers.FirstAsync(l => l.Id == UniversityDbContext.LecturerNowakId);

            await context.Grades.AddAsync(new Grade
            {
                Id = UniversityDbContext.GradeSeedId,
                Student = student,
                Course = course,
                AcademicYear = year,
                Instructor = lecturer,
                Date = new DateTime(2026, 5, 9, 0, 0, 0, DateTimeKind.Utc),
                GradeType = GradeType.Partial,
                GradeValue = GradeValue.Grade40
            });

            await context.SaveChangesAsync();
        }
    }
}
