using CoreApp.Application.Dto.Grades;
using CoreApp.Application.Security;
using CoreApp.Application.Services;
using CoreApp.Domain.Enums;
using Infrastructure.Memory;
using Tests.Fakes;
using Xunit;

namespace Tests;

public class StudentServiceTests
{
    [Fact]
    public async Task AddGrade_AllowsCourseLecturer_AndCreatesHistory()
    {
        var fixture = CreateFixture(LecturerUser("jan.nowak@wsei.edu.pl"));
        var dto = CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId);

        var result = await fixture.Service.AddGrade(MemoryStudentRepository.FirstStudentId, dto);
        var student = await fixture.Students.FindByIdAsync(MemoryStudentRepository.FirstStudentId);
        var grade = student!.Grades.Single(g => g.Id == result.Id);

        Assert.Equal(4.0, result.GradeValue);
        Assert.Single(grade.History);
        Assert.Equal("Added", grade.History[0].ActionType);
        Assert.Null(grade.History[0].OldValue);
        Assert.Equal(GradeValue.Grade40, grade.History[0].NewValue);
        Assert.Equal("lecturer-user", grade.History[0].ChangedByUserId);
        Assert.Equal("jan.nowak@wsei.edu.pl", grade.History[0].ChangedByUserName);
    }

    [Fact]
    public async Task AddGrade_AllowsDeanOfficeStaff()
    {
        var fixture = CreateFixture(DeanOfficeUser());
        var dto = CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId);

        var result = await fixture.Service.AddGrade(MemoryStudentRepository.FirstStudentId, dto);

        Assert.Equal(MemoryCourseRepository.AlgorithmsCourseId, result.CourseId);
    }

    [Fact]
    public async Task AddGrade_RejectsLecturerWhoDoesNotMatchSelectedLecturerId()
    {
        var fixture = CreateFixture(LecturerUser("anna.kowalska@wsei.edu.pl"));
        var dto = CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            fixture.Service.AddGrade(MemoryStudentRepository.FirstStudentId, dto));
    }

    [Fact]
    public async Task AddGrade_RejectsStudentNotEnrolledInCourse()
    {
        var fixture = CreateFixture(LecturerUser("jan.nowak@wsei.edu.pl"));
        var dto = CreateGradeDto(MemoryStudentRepository.SecondStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            fixture.Service.AddGrade(MemoryStudentRepository.SecondStudentId, dto));
    }

    [Fact]
    public async Task AddGrade_RejectsLecturerIdThatDoesNotTeachCourse()
    {
        var fixture = CreateFixture(DeanOfficeUser());
        var dto = CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerKowalskaId);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            fixture.Service.AddGrade(MemoryStudentRepository.FirstStudentId, dto));
    }

    [Fact]
    public async Task UpdateGrade_AllowsCourseLecturer_AndCreatesHistory()
    {
        var fixture = CreateFixture(LecturerUser("jan.nowak@wsei.edu.pl"));
        var added = await fixture.Service.AddGrade(
            MemoryStudentRepository.FirstStudentId,
            CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId));

        var updated = await fixture.Service.UpdateGrade(
            MemoryStudentRepository.FirstStudentId,
            added.Id,
            new GradeUpdateDto
            {
                Date = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                GradeType = GradeType.Final,
                GradeValue = 5.0
            });

        var student = await fixture.Students.FindByIdAsync(MemoryStudentRepository.FirstStudentId);
        var grade = student!.Grades.Single(g => g.Id == added.Id);

        Assert.Equal(5.0, updated.GradeValue);
        Assert.Equal(2, grade.History.Count);
        Assert.Equal("Updated", grade.History[1].ActionType);
        Assert.Equal(GradeValue.Grade40, grade.History[1].OldValue);
        Assert.Equal(GradeValue.Grade50, grade.History[1].NewValue);
    }

    [Fact]
    public async Task UpdateGrade_AllowsDeanOfficeStaff()
    {
        var fixture = CreateFixture(LecturerUser("jan.nowak@wsei.edu.pl"));
        var added = await fixture.Service.AddGrade(
            MemoryStudentRepository.FirstStudentId,
            CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId));

        var deanService = CreateService(fixture, DeanOfficeUser());

        var updated = await deanService.UpdateGrade(
            MemoryStudentRepository.FirstStudentId,
            added.Id,
            new GradeUpdateDto
            {
                Date = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                GradeType = GradeType.Final,
                GradeValue = 4.5
            });

        Assert.Equal(4.5, updated.GradeValue);
    }

    [Fact]
    public async Task UpdateGrade_RejectsLecturerWhoDoesNotTeachCourse()
    {
        var fixture = CreateFixture(LecturerUser("jan.nowak@wsei.edu.pl"));
        var added = await fixture.Service.AddGrade(
            MemoryStudentRepository.FirstStudentId,
            CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId));

        var foreignLecturerService = CreateService(fixture, LecturerUser("anna.kowalska@wsei.edu.pl"));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            foreignLecturerService.UpdateGrade(
                MemoryStudentRepository.FirstStudentId,
                added.Id,
                new GradeUpdateDto
                {
                    Date = new DateTime(2026, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    GradeType = GradeType.Final,
                    GradeValue = 5.0
                }));
    }

    [Fact]
    public async Task GetGrades_ReturnsStudentGrades()
    {
        var fixture = CreateFixture(LecturerUser("jan.nowak@wsei.edu.pl"));
        await fixture.Service.AddGrade(
            MemoryStudentRepository.FirstStudentId,
            CreateGradeDto(MemoryStudentRepository.FirstStudentId, MemoryCourseRepository.AlgorithmsCourseId, MemoryLecturerRepository.LecturerNowakId));

        var grades = await fixture.Service.GetGrades(MemoryStudentRepository.FirstStudentId);

        Assert.Single(grades);
    }

    private static ServiceFixture CreateFixture(FakeCurrentUserContext user)
    {
        var students = new MemoryStudentRepository();
        var lecturers = new MemoryLecturerRepository();
        var courses = new MemoryCourseRepository();
        var academicYears = new MemoryAcademicYearRepository();
        var grades = new MemoryGradeRepository();
        var unitOfWork = new MemoryUniversityUnitOfWork(students, lecturers, courses, academicYears, grades);
        var service = new StudentService(unitOfWork, user);

        return new ServiceFixture(students, lecturers, courses, academicYears, grades, unitOfWork, service);
    }

    private static StudentService CreateService(ServiceFixture fixture, FakeCurrentUserContext user)
    {
        return new StudentService(fixture.UnitOfWork, user);
    }

    private static GradeDto CreateGradeDto(Guid studentId, Guid courseId, Guid lecturerId)
    {
        _ = studentId;
        return new GradeDto
        {
            CourseId = courseId,
            LecturerId = lecturerId,
            AcademicYearId = MemoryAcademicYearRepository.AcademicYear2025Id,
            Date = new DateTime(2026, 5, 10, 0, 0, 0, DateTimeKind.Utc),
            GradeType = GradeType.Partial,
            GradeValue = 4.0
        };
    }

    private static FakeCurrentUserContext LecturerUser(string email)
    {
        return new FakeCurrentUserContext("lecturer-user", email, UserRole.Lecturer.ToString());
    }

    private static FakeCurrentUserContext DeanOfficeUser()
    {
        return new FakeCurrentUserContext("dean-user", "dean@wsei.edu.pl", UserRole.DeanOfficeStaff.ToString());
    }

    private sealed record ServiceFixture(
        MemoryStudentRepository Students,
        MemoryLecturerRepository Lecturers,
        MemoryCourseRepository Courses,
        MemoryAcademicYearRepository AcademicYears,
        MemoryGradeRepository Grades,
        MemoryUniversityUnitOfWork UnitOfWork,
        StudentService Service);
}
