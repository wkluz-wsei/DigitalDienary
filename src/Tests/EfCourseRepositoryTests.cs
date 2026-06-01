using Infrastructure.EntityFramework.Context;
using Infrastructure.EntityFramework.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests;

public class EfCourseRepositoryTests
{
    [Fact]
    public async Task HasStudentEnrollmentAsync_ReturnsTrue_ForSeededEnrollment()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var result = await repository.HasStudentEnrollmentAsync(
            UniversityDbContext.CourseAlgorithmsId,
            UniversityDbContext.StudentAdamId);

        Assert.True(result);
    }

    [Fact]
    public async Task HasStudentEnrollmentAsync_ReturnsFalse_ForStudentNotEnrolledInCourse()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var result = await repository.HasStudentEnrollmentAsync(
            UniversityDbContext.CourseAlgorithmsId,
            UniversityDbContext.StudentEwaId);

        Assert.False(result);
    }

    [Fact]
    public async Task IsTaughtByLecturerEmailAsync_ReturnsTrue_ForSeededLecturerCourse()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var result = await repository.IsTaughtByLecturerEmailAsync(
            UniversityDbContext.CourseAlgorithmsId,
            "jan.nowak@wsei.edu.pl");

        Assert.True(result);
    }

    [Fact]
    public async Task IsTaughtByLecturerEmailAsync_ReturnsFalse_ForForeignCourse()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var result = await repository.IsTaughtByLecturerEmailAsync(
            UniversityDbContext.CourseDatabasesId,
            "jan.nowak@wsei.edu.pl");

        Assert.False(result);
    }

    [Fact]
    public async Task IsTaughtByLecturerIdAsync_ReturnsTrue_ForSeededLecturerCourse()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var result = await repository.IsTaughtByLecturerIdAsync(
            UniversityDbContext.CourseDatabasesId,
            UniversityDbContext.LecturerKowalskaId);

        Assert.True(result);
    }

    [Fact]
    public async Task FindByIdWithEnrollmentsAsync_LoadsEnrollments()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var course = await repository.FindByIdWithEnrollmentsAsync(UniversityDbContext.CourseDatabasesId);

        Assert.NotNull(course);
        Assert.Equal(2, course!.Enrollments.Count);
        Assert.Contains(course.Enrollments, s => s.Id == UniversityDbContext.StudentAdamId);
        Assert.Contains(course.Enrollments, s => s.Id == UniversityDbContext.StudentEwaId);
    }

    [Fact]
    public async Task FindByLecturerEmailWithEnrollmentsAsync_LoadsOnlyLecturerCourses()
    {
        await using var fixture = await EfFixture.CreateAsync();
        var repository = new EfCourseRepository(fixture.Context);

        var courses = (await repository.FindByLecturerEmailWithEnrollmentsAsync("jan.nowak@wsei.edu.pl")).ToList();

        Assert.Single(courses);
        Assert.Equal(UniversityDbContext.CourseAlgorithmsId, courses[0].Id);
        Assert.Single(courses[0].Enrollments);
    }

    private sealed class EfFixture : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private EfFixture(SqliteConnection connection, UniversityDbContext context)
        {
            _connection = connection;
            Context = context;
        }

        public UniversityDbContext Context { get; }

        public static async Task<EfFixture> CreateAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<UniversityDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new UniversityDbContext(options);
            await context.Database.EnsureCreatedAsync();

            return new EfFixture(connection, context);
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }
}
