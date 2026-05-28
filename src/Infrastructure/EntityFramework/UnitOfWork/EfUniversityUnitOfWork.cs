using CoreApp.Application.Repositories;
using CoreApp.Application.UnitOfWork;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.UnitOfWork;

public class EfUniversityUnitOfWork(
    IStudentRepository students,
    ILecturerRepository lecturers,
    ICourseRepository courses,
    IAcademicYearRepository academicYears,
    IGradeRepository grades,
    UniversityDbContext context) : IUniversityUnitOfWork
{
    public IStudentRepository Students => students;
    public ILecturerRepository Lecturers => lecturers;
    public ICourseRepository Courses => courses;
    public IAcademicYearRepository AcademicYears => academicYears;
    public IGradeRepository Grades => grades;

    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await context.Database.CommitTransactionAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await context.Database.RollbackTransactionAsync();
    }
}
