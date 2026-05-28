using CoreApp.Domain.Enums;
using CoreApp.Domain.Entities;

namespace CoreApp.Application.Dto.Students;

public sealed record StudentUpdateDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string NationalId { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string StudentId { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }

    public void UpdateEntity(Student student)
    {
        student.FirstName = FirstName;
        student.LastName = LastName;
        student.NationalId = NationalId;
        student.Email = Email;
        student.StudentId = StudentId;
        student.ProgramName = ProgramName;
        student.YearOfStudy = YearOfStudy;
        student.Status = Status;
    }
}
