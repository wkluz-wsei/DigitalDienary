using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace Infrastructure.Memory;

public class MemoryCourseRepository : MemoryGenericRepository<Course>, ICourseRepository
{
    public static readonly Guid AlgorithmsCourseId = Guid.Parse("8e8f8f66-bb84-4f77-8a42-6052b8c64410");
    public static readonly Guid DatabasesCourseId = Guid.Parse("1bcf84f4-57fa-4fb1-a655-bd8ee4d061d2");

    public MemoryCourseRepository()
    {
        _data.Add(AlgorithmsCourseId, new Course
        {
            Id = AlgorithmsCourseId,
            Code = "ALG101",
            Name = "Algorytmy i struktury danych",
            EctsCredits = 6,
            CompletionType = CompletionType.Exam,
            Semester = Semester.Winter
        });

        _data.Add(DatabasesCourseId, new Course
        {
            Id = DatabasesCourseId,
            Code = "DBS201",
            Name = "Bazy danych",
            EctsCredits = 5,
            CompletionType = CompletionType.CreditWithGrade,
            Semester = Semester.Summer
        });
    }
}
