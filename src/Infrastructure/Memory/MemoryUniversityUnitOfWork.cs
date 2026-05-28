using CoreApp.Application.Repositories;
using CoreApp.Application.UnitOfWork;

namespace Infrastructure.Memory;

public class MemoryUniversityUnitOfWork(
    IStudentRepository students,
    ILecturerRepository lecturers,
    ICourseRepository courses,
    IAcademicYearRepository academicYears,
    IGradeRepository grades) : IUniversityUnitOfWork
{
    public IStudentRepository Students => students;
    public ILecturerRepository Lecturers => lecturers;
    public ICourseRepository Courses => courses;
    public IAcademicYearRepository AcademicYears => academicYears;
    public IGradeRepository Grades => grades;

    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task CommitTransactionAsync()
    {
        return Task.CompletedTask;
    }

    public Task RollbackTransactionAsync()
    {
        return Task.CompletedTask;
    }
}
