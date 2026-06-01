using CoreApp.Domain.Enums;

namespace CoreApp.Domain.Entities;

public class GradeHistory : EntityBase
{
    public Guid GradeId { get; set; }
    public Grade Grade { get; set; } = null!;

    public GradeValue? OldValue { get; set; }
    public GradeValue NewValue { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;
    public string ChangedByUserName { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public string? ActionType { get; set; } // "Added", "Updated"
}
