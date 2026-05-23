using CoreApp.Domain.Enums;

namespace CoreApp.Domain.Entities;

public class Course : EntityBase
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int EctsCredits { get; set; }

    public CompletionType CompletionType { get; set; }
    public Semester Semester { get; set; }

    public List<Student> Enrollments { get; set; } = new();
}