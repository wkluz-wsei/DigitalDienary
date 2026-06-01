using CoreApp.Application.Security;
using CoreApp.Application.Services;
using Infrastructure.Memory;
using Tests.Fakes;
using Xunit;

namespace Tests;

public class LecturerServiceTests
{
    [Fact]
    public async Task GetStudents_ReturnsStudentsFromCurrentLecturerCourses()
    {
        var service = CreateService(LecturerUser("jan.nowak@wsei.edu.pl"));

        var students = (await service.GetStudentsAsync()).ToList();

        Assert.Single(students);
        Assert.Equal("S001", students[0].StudentId);
    }

    [Fact]
    public async Task GetStudents_ReturnsAllStudentsForDeanOfficeStaff()
    {
        var service = CreateService(DeanOfficeUser());

        var students = (await service.GetStudentsAsync()).ToList();

        Assert.Equal(2, students.Count);
        Assert.Contains(students, s => s.StudentId == "S001");
        Assert.Contains(students, s => s.StudentId == "S002");
    }

    [Fact]
    public async Task GetStudentsForCourse_ReturnsStudentsForOwnCourse()
    {
        var service = CreateService(LecturerUser("jan.nowak@wsei.edu.pl"));

        var students = (await service.GetStudentsForCourseAsync(MemoryCourseRepository.AlgorithmsCourseId)).ToList();

        Assert.Single(students);
        Assert.Equal("S001", students[0].StudentId);
    }

    [Fact]
    public async Task GetStudentsForCourse_RejectsLecturerForForeignCourse()
    {
        var service = CreateService(LecturerUser("jan.nowak@wsei.edu.pl"));

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.GetStudentsForCourseAsync(MemoryCourseRepository.DatabasesCourseId));
    }

    [Fact]
    public async Task GetStudentsForCourse_AllowsDeanOfficeStaffForAnyCourse()
    {
        var service = CreateService(DeanOfficeUser());

        var students = (await service.GetStudentsForCourseAsync(MemoryCourseRepository.DatabasesCourseId)).ToList();

        Assert.Equal(2, students.Count);
    }

    [Fact]
    public async Task GetStudents_ReturnsEmptyListForLecturerWithoutCourses()
    {
        var service = CreateService(LecturerUser("unknown@wsei.edu.pl"));

        var students = await service.GetStudentsAsync();

        Assert.Empty(students);
    }

    private static LecturerService CreateService(FakeCurrentUserContext user)
    {
        var unitOfWork = new MemoryUniversityUnitOfWork(
            new MemoryStudentRepository(),
            new MemoryLecturerRepository(),
            new MemoryCourseRepository(),
            new MemoryAcademicYearRepository(),
            new MemoryGradeRepository());

        return new LecturerService(unitOfWork, user);
    }

    private static FakeCurrentUserContext LecturerUser(string email)
    {
        return new FakeCurrentUserContext("lecturer-user", email, UserRole.Lecturer.ToString());
    }

    private static FakeCurrentUserContext DeanOfficeUser()
    {
        return new FakeCurrentUserContext("dean-user", "dean@wsei.edu.pl", UserRole.DeanOfficeStaff.ToString());
    }
}
