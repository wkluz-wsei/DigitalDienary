using CoreApp.Domain.ValueObjects;
using Xunit;

namespace Tests;

public class PeselTests
{
    [Theory]
    [InlineData("99010112342")]
    [InlineData("00210112344")]
    [InlineData("80810112344")]
    public void IsValid_ReturnsTrue_ForValidPesel(string value)
    {
        Assert.True(PESEL.IsValid(value));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123")]
    [InlineData("9901011234A")]
    [InlineData("99010112343")]
    [InlineData("99000112341")]
    public void IsValid_ReturnsFalse_ForInvalidPesel(string? value)
    {
        Assert.False(PESEL.IsValid(value));
    }

    [Fact]
    public void Constructor_Throws_ForInvalidPesel()
    {
        Assert.Throws<ArgumentException>(() => new PESEL("99010112343"));
    }

    [Fact]
    public void TryCreate_ReturnsPesel_ForValidValue()
    {
        var result = PESEL.TryCreate("99010112342", out var pesel);

        Assert.True(result);
        Assert.NotNull(pesel);
        Assert.Equal("99010112342", pesel!.Value);
    }

    [Fact]
    public void TryCreate_ReturnsFalse_ForInvalidValue()
    {
        var result = PESEL.TryCreate("99010112343", out var pesel);

        Assert.False(result);
        Assert.Null(pesel);
    }

    [Theory]
    [InlineData("99010112342", 1999, 1, 1)]
    [InlineData("00210112344", 2000, 1, 1)]
    [InlineData("80810112344", 1880, 1, 1)]
    public void GetBirthDate_ReturnsExpectedDate(string value, int year, int month, int day)
    {
        var pesel = new PESEL(value);

        Assert.Equal(new DateOnly(year, month, day), pesel.GetBirthDate());
    }

    [Theory]
    [InlineData("99010112342", "Female")]
    [InlineData("99010112359", "Male")]
    public void GetGender_ReturnsExpectedGender(string value, string expectedGender)
    {
        var pesel = new PESEL(value);

        Assert.Equal(expectedGender, pesel.GetGender());
    }

    [Fact]
    public void ToString_ReturnsRawValue()
    {
        var pesel = new PESEL("99010112342");

        Assert.Equal("99010112342", pesel.ToString());
    }

    [Fact]
    public void ImplicitStringConversion_ReturnsRawValue()
    {
        var pesel = new PESEL("99010112342");

        string value = pesel;

        Assert.Equal("99010112342", value);
    }
}
