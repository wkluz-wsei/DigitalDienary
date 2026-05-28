using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace Infrastructure.Memory;

public class MemoryStudentRepository : MemoryGenericRepository<Student>, IStudentRepository
{
    public static readonly Guid FirstStudentId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f");
    public static readonly Guid SecondStudentId = Guid.Parse("7ba19ea5-3967-4b24-bb87-b14c9ee14770");

    public MemoryStudentRepository()
    {
        var firstStudent = new Student
        {
            Id = FirstStudentId,
            FirstName = "Adam",
            LastName = "Nowak",
            NationalId = "99010112345",
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
            NationalId = "98020254321",
            Email = "ewa.kowalska@example.com",
            StudentId = "S002",
            ProgramName = "Matematyka",
            YearOfStudy = 2,
            Status = StudentStatus.OnLeave
        };

        _data.Add(firstStudent.Id, firstStudent);
        _data.Add(secondStudent.Id, secondStudent);
    }

    public Task<IEnumerable<Student>> GetStudentsByStudyYearAsync(int studyYear)
    {
        var students = _data.Values
            .Where(student => student.YearOfStudy == studyYear)
            .AsEnumerable();

        return Task.FromResult(students);
    }

    public Task<IEnumerable<Student>> GetStudentsByStatusAsync(StudentStatus status)
    {
        var students = _data.Values
            .Where(student => student.Status == status)
            .AsEnumerable();

        return Task.FromResult(students);
    }
}
