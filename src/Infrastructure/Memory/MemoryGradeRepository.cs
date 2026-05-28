using CoreApp.Application.Repositories;
using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace Infrastructure.Memory;

public class MemoryGradeRepository : MemoryGenericRepository<Grade>, IGradeRepository
{
    public MemoryGradeRepository()
    {
        var student = new Student
        {
            Id = MemoryStudentRepository.FirstStudentId,
            FirstName = "Adam",
            LastName = "Nowak",
            NationalId = "99010112345",
            Email = "adam.nowak@example.com",
            StudentId = "S001",
            ProgramName = "Informatyka",
            YearOfStudy = 1,
            Status = StudentStatus.Active
        };

        var grade = new Grade
        {
            Id = Guid.Parse("f063f470-e91d-447a-96ae-132286d4ca77"),
            Student = student,
            Course = new Course
            {
                Id = MemoryCourseRepository.AlgorithmsCourseId,
                Code = "ALG101",
                Name = "Algorytmy i struktury danych",
                EctsCredits = 6,
                CompletionType = CompletionType.Exam,
                Semester = Semester.Winter
            },
            AcademicYear = new AcademicYear
            {
                Id = MemoryAcademicYearRepository.AcademicYear2025Id,
                Name = "2025/2026"
            },
            Instructor = new Lecturer
            {
                Id = MemoryLecturerRepository.LecturerNowakId,
                FirstName = "Jan",
                LastName = "Nowak",
                NationalId = "75010112345",
                Email = "jan.nowak@wsei.edu.pl",
                Title = "dr inż.",
                Faculty = "Informatyka"
            },
            Date = new DateTime(2026, 5, 9),
            GradeType = GradeType.Partial,
            GradeValue = GradeValue.Grade40
        };

        _data.Add(grade.Id, grade);
    }
}
