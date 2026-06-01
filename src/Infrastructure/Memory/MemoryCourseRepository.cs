using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;
using CoreApp.Domain.ValueObjects;

namespace Infrastructure.Memory;

public class MemoryCourseRepository : MemoryGenericRepository<Course>, ICourseRepository
{
    public static readonly Guid AlgorithmsCourseId = Guid.Parse("8e8f8f66-bb84-4f77-8a42-6052b8c64410");
    public static readonly Guid DatabasesCourseId = Guid.Parse("1bcf84f4-57fa-4fb1-a655-bd8ee4d061d2");

    private static readonly Guid FirstStudentId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f");
    private static readonly Guid SecondStudentId = Guid.Parse("7ba19ea5-3967-4b24-bb87-b14c9ee14770");
    private static readonly Guid LecturerNowakId = Guid.Parse("0d7c4923-0ed0-4db9-a392-4b3c520ef77e");
    private static readonly Guid LecturerKowalskaId = Guid.Parse("05f64714-3ac1-4327-8e02-fd519844126f");

    public MemoryCourseRepository()
    {
        var firstStudent = new Student
        {
            Id = FirstStudentId,
            FirstName = "Adam",
            LastName = "Nowak",
            NationalId = new PESEL("99010112342"),
            Email = "adam.nowak@example.com",
            StudentId = "S001",
            ProgramName = "Informatyka",
            YearOfStudy = 1,
            Status = StudentStatus.Active
        };

        var secondStudent = new Student
        {
            Id = SecondStudentId,
            FirstName = "Ewa",
            LastName = "Kowalska",
            NationalId = new PESEL("98020254323"),
            Email = "ewa.kowalska@example.com",
            StudentId = "S002",
            ProgramName = "Matematyka",
            YearOfStudy = 2,
            Status = StudentStatus.OnLeave
        };

        _data.Add(AlgorithmsCourseId, new Course
        {
            Id = AlgorithmsCourseId,
            Code = "ALG101",
            Name = "Algorytmy i struktury danych",
            EctsCredits = 6,
            CompletionType = CompletionType.Exam,
            Semester = Semester.Winter,
            Enrollments = new List<Student> { firstStudent }
        });

        _data.Add(DatabasesCourseId, new Course
        {
            Id = DatabasesCourseId,
            Code = "DBS201",
            Name = "Bazy danych",
            EctsCredits = 5,
            CompletionType = CompletionType.CreditWithGrade,
            Semester = Semester.Summer,
            Enrollments = new List<Student> { firstStudent, secondStudent }
        });
    }

    public Task<Course?> FindByIdWithEnrollmentsAsync(Guid id)
    {
        return FindByIdAsync(id);
    }

    public Task<IEnumerable<Course>> FindByLecturerEmailWithEnrollmentsAsync(string email)
    {
        var courses = _data.Values
            .Where(c => IsTaughtByLecturerEmail(c.Id, email))
            .AsEnumerable();

        return Task.FromResult(courses);
    }

    public Task<bool> HasStudentEnrollmentAsync(Guid courseId, Guid studentId)
    {
        var result = _data.TryGetValue(courseId, out var course)
            && course.Enrollments.Any(s => s.Id == studentId);

        return Task.FromResult(result);
    }

    public Task<bool> IsTaughtByLecturerEmailAsync(Guid courseId, string email)
    {
        return Task.FromResult(IsTaughtByLecturerEmail(courseId, email));
    }

    public Task<bool> IsTaughtByLecturerIdAsync(Guid courseId, Guid lecturerId)
    {
        var result = courseId == AlgorithmsCourseId && lecturerId == LecturerNowakId
            || courseId == DatabasesCourseId && lecturerId == LecturerKowalskaId;

        return Task.FromResult(result);
    }

    private static bool IsTaughtByLecturerEmail(Guid courseId, string email)
    {
        return courseId == AlgorithmsCourseId && email == "jan.nowak@wsei.edu.pl"
            || courseId == DatabasesCourseId && email == "anna.kowalska@wsei.edu.pl";
    }
}
