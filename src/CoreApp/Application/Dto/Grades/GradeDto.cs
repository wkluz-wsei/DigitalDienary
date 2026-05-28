using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Dto.Grades;

public sealed record GradeDto
{
    public Guid Id { get; init; }
    public Guid CourseId { get; init; }
    public Guid LecturerId { get; init; }
    public Guid AcademicYearId { get; init; }
    public DateTime Date { get; init; }
    public GradeType GradeType { get; init; }
    public double GradeValue { get; init; }

    public static GradeDto FromEntity(Grade grade)
    {
        return new GradeDto
        {
            Id = grade.Id,
            CourseId = grade.Course.Id,
            LecturerId = grade.Instructor?.Id ?? Guid.Empty,
            AcademicYearId = grade.AcademicYear.Id,
            Date = grade.Date,
            GradeType = grade.GradeType,
            GradeValue = GradeValueMapper.ToDouble(grade.GradeValue)
        };
    }
}

public static class GradeValueMapper
{
    public static GradeValue ToEnum(double value)
    {
        return value switch
        {
            2.0 => GradeValue.Grade20,
            3.0 => GradeValue.Grade30,
            3.5 => GradeValue.Grade35,
            4.0 => GradeValue.Grade40,
            4.5 => GradeValue.Grade45,
            5.0 => GradeValue.Grade50,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported grade value.")
        };
    }

    public static double ToDouble(GradeValue value)
    {
        return value switch
        {
            GradeValue.Grade20 => 2.0,
            GradeValue.Grade30 => 3.0,
            GradeValue.Grade35 => 3.5,
            GradeValue.Grade40 => 4.0,
            GradeValue.Grade45 => 4.5,
            GradeValue.Grade50 => 5.0,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, "Unsupported grade value.")
        };
    }
}
