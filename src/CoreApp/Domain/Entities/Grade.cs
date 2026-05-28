using CoreApp.Domain.Enums;

namespace CoreApp.Domain.Entities;

public class Grade : EntityBase
{
    public Student Student { get; set; } = null!;
    public Course Course { get; set; } = null!;

    public AcademicYear AcademicYear { get; set; } = null!;

    public DateTime Date { get; set; }

    public GradeType GradeType { get; set; }
    public GradeValue GradeValue { get; set; }

    public Lecturer? Instructor { get; set; }
}