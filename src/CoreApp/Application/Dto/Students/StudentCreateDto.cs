using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Dto.Students;

public sealed record StudentCreateDto : PersonCreateDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public int YearOfStudy { get; init; } = 1;
    public StudentStatus Status { get; init; } = StudentStatus.Active;

    public static Student ToEntity(StudentCreateDto student)
    {
        return new Student
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            NationalId = student.NationalId,
            Email = student.Email,
            StudentId = student.StudentId,
            ProgramName = student.ProgramName,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status
        };
    }
}
