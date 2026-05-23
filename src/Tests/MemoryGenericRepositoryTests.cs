using CoreApp.Domain.Entities;
using CoreApp.Application.Repositories;
using Infrastructure.Memory;

namespace Tests;

public class MemoryGenericRepositoryTests
{
    private readonly IGenericRepositoryAsync<Student> _repo =
        new MemoryGenericRepository<Student>();

    [Fact]
    public async Task AddStudentToRepositoryTestAsync()
    {
        var student = new Student
        {
            FirstName = "Adam",
            LastName = "Nowak",
            Email = "adam@test.com",
            StudentId = "s123"
        };

        await _repo.AddAsync(student);

        var result = await _repo.FindByIdAsync(student.Id);

        Assert.NotNull(result);
        Assert.Equal(student.Id, result!.Id);
    }
}