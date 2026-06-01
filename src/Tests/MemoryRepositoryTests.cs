using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using CoreApp.Domain.ValueObjects;
using Infrastructure.Memory;
using Xunit;

namespace Tests;

public class MemoryRepositoryTests
{
    private readonly IGenericRepositoryAsync<Student> _repo = new MemoryGenericRepository<Student>();

    [Fact]
    public async Task AddAsync_StoresEntity()
    {
        var student = CreateStudent("S123");

        await _repo.AddAsync(student);

        var result = await _repo.FindByIdAsync(student.Id);
        Assert.NotNull(result);
        Assert.Equal(student.Id, result!.Id);
    }

    [Fact]
    public async Task FindAllAsync_ReturnsStoredEntities()
    {
        await _repo.AddAsync(CreateStudent("S001"));
        await _repo.AddAsync(CreateStudent("S002"));

        var result = await _repo.FindAllAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task FindPagedAsync_ReturnsRequestedPage()
    {
        await _repo.AddAsync(CreateStudent("S001"));
        await _repo.AddAsync(CreateStudent("S002"));
        await _repo.AddAsync(CreateStudent("S003"));

        var result = await _repo.FindPagedAsync(2, 1);

        Assert.Single(result.Items);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(1, result.PageSize);
    }

    [Fact]
    public async Task UpdateAsync_ReplacesEntity()
    {
        var student = CreateStudent("S001");
        await _repo.AddAsync(student);
        student.LastName = "Changed";

        await _repo.UpdateAsync(student);

        var result = await _repo.FindByIdAsync(student.Id);
        Assert.Equal("Changed", result!.LastName);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenEntityDoesNotExist()
    {
        var student = CreateStudent("S001");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.UpdateAsync(student));
    }

    [Fact]
    public async Task RemoveByIdAsync_RemovesEntity()
    {
        var student = CreateStudent("S001");
        await _repo.AddAsync(student);

        await _repo.RemoveByIdAsync(student.Id);

        Assert.Null(await _repo.FindByIdAsync(student.Id));
    }

    [Fact]
    public async Task MemoryCourseRepository_HasSeededEnrollments()
    {
        var repository = new MemoryCourseRepository();

        Assert.True(await repository.HasStudentEnrollmentAsync(
            MemoryCourseRepository.AlgorithmsCourseId,
            MemoryStudentRepository.FirstStudentId));
        Assert.False(await repository.HasStudentEnrollmentAsync(
            MemoryCourseRepository.AlgorithmsCourseId,
            MemoryStudentRepository.SecondStudentId));
    }

    [Fact]
    public async Task MemoryCourseRepository_HasSeededLecturerCourses()
    {
        var repository = new MemoryCourseRepository();

        Assert.True(await repository.IsTaughtByLecturerEmailAsync(
            MemoryCourseRepository.AlgorithmsCourseId,
            "jan.nowak@wsei.edu.pl"));
        Assert.False(await repository.IsTaughtByLecturerEmailAsync(
            MemoryCourseRepository.DatabasesCourseId,
            "jan.nowak@wsei.edu.pl"));
    }

    private static Student CreateStudent(string studentId)
    {
        return new Student
        {
            NationalId = new PESEL("99010112342"),
            FirstName = "Adam",
            LastName = "Nowak",
            Email = $"{studentId}@test.com",
            StudentId = studentId
        };
    }
}
