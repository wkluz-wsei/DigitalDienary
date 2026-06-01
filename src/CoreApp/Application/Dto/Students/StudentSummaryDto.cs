using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Dto.Students;

public sealed record StudentSummaryDto : PersonDto
{
    public string StudentId { get; init; } = string.Empty;
    public string ProgramName { get; init; } = string.Empty;
    public int YearOfStudy { get; init; }
    public StudentStatus Status { get; init; }

    public static StudentSummaryDto FromEntity(Student student)
    {
        return new StudentSummaryDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            NationalId = student.NationalId?.Value ?? string.Empty,
            Email = student.Email,
            StudentId = student.StudentId,
            ProgramName = student.ProgramName,
            YearOfStudy = student.YearOfStudy,
            Status = student.Status
        };
    }
}
