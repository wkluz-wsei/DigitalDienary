using CoreApp.Application.Repositories;

namespace CoreApp.Application.UnitOfWork;

public interface IUniversityUnitOfWork : IAsyncDisposable
{
    IStudentRepository Students { get; }
    ILecturerRepository Lecturers { get; }
    ICourseRepository Courses { get; }
    IAcademicYearRepository AcademicYears { get; }
    IGradeRepository Grades { get; }

    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
