using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Dto.Students;

public sealed record StudentDetailDto : PersonDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }
    public int GradeCount { get; init; }

    public static StudentDetailDto FromEntity(Student student)
    {
        return new StudentDetailDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            NationalId = student.NationalId?.Value ?? string.Empty,
            Email = student.Email,
            StudentId = student.StudentId,
            ProgramName = student.ProgramName,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status,
            GradeCount = student.Grades.Count
        };
    }
}
