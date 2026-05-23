using CoreApp.Domain.Enums;

namespace CoreApp.Application.Dto.Students;

public sealed record StudentStatusUpdateDto
{
    public StudentStatus Status { get; init; }
}
