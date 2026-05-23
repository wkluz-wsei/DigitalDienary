using CoreApp.Domain.Enums;

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
}
