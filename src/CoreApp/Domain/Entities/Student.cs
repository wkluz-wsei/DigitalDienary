using CoreApp.Domain.Enums;

namespace CoreApp.Domain.Entities;

public class Student : Person
{
    public string StudentId { get; set; } = "";
    public int YearOfStudy { get; set; }
    public string ProgramName { get; set; } = "";
    public StudentStatus Status { get; set; }

    public List<Grade> Grades { get; set; } = new();
}