using CoreApp.Application.Dto.Grades;
using CoreApp.Domain.Enums;
using Xunit;

namespace Tests;

public class GradeValueMapperTests
{
    [Theory]
    [InlineData(2.0, GradeValue.Grade20)]
    [InlineData(3.0, GradeValue.Grade30)]
    [InlineData(3.5, GradeValue.Grade35)]
    [InlineData(4.0, GradeValue.Grade40)]
    [InlineData(4.5, GradeValue.Grade45)]
    [InlineData(5.0, GradeValue.Grade50)]
    public void ToEnum_MapsSupportedValues(double value, GradeValue expected)
    {
        Assert.Equal(expected, GradeValueMapper.ToEnum(value));
    }

    [Theory]
    [InlineData(GradeValue.Grade20, 2.0)]
    [InlineData(GradeValue.Grade30, 3.0)]
    [InlineData(GradeValue.Grade35, 3.5)]
    [InlineData(GradeValue.Grade40, 4.0)]
    [InlineData(GradeValue.Grade45, 4.5)]
    [InlineData(GradeValue.Grade50, 5.0)]
    public void ToDouble_MapsSupportedValues(GradeValue value, double expected)
    {
        Assert.Equal(expected, GradeValueMapper.ToDouble(value));
    }

    [Fact]
    public void ToEnum_Throws_ForUnsupportedValue()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => GradeValueMapper.ToEnum(4.25));
    }
}
