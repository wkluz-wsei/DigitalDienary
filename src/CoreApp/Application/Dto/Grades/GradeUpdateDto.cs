using CoreApp.Domain.Entities;
using CoreApp.Domain.Enums;

namespace CoreApp.Application.Dto.Grades;

public sealed record GradeUpdateDto
{
    public DateTime Date { get; init; }
    public GradeType GradeType { get; init; }
    public double GradeValue { get; init; }

    public void UpdateEntity(Grade grade)
    {
        grade.Date = Date;
        grade.GradeType = GradeType;
        grade.GradeValue = GradeValueMapper.ToEnum(GradeValue);
    }
}
