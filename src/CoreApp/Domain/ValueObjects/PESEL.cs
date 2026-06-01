using System.Text.RegularExpressions;

namespace CoreApp.Domain.ValueObjects;

public sealed record PESEL
{
    private static readonly int[] Weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

    public string Value { get; init; }

    public PESEL(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Invalid PESEL format or checksum.", nameof(value));

        Value = value;
    }

    public static bool IsValid(string? pesel)
    {
        if (string.IsNullOrWhiteSpace(pesel) || pesel.Length != 11 || !Regex.IsMatch(pesel, @"^\d{11}$"))
            return false;

        if (!TryGetBirthDate(pesel, out _))
            return false;

        var sum = 0;
        for (var i = 0; i < Weights.Length; i++)
        {
            sum += (pesel[i] - '0') * Weights[i];
        }

        var checksum = (10 - sum % 10) % 10;
        return checksum == pesel[10] - '0';
    }

    public static bool TryCreate(string? value, out PESEL? pesel)
    {
        if (!IsValid(value))
        {
            pesel = null;
            return false;
        }

        pesel = new PESEL(value!);
        return true;
    }

    public DateOnly GetBirthDate()
    {
        if (!TryGetBirthDate(Value, out var birthDate))
            throw new InvalidOperationException("PESEL contains invalid birth date.");

        return birthDate;
    }

    public string GetGender()
    {
        return (Value[9] - '0') % 2 == 0 ? "Female" : "Male";
    }

    public override string ToString() => Value;

    public static implicit operator string(PESEL pesel) => pesel.Value;
    public static explicit operator PESEL(string value) => new(value);

    private static bool TryGetBirthDate(string pesel, out DateOnly birthDate)
    {
        birthDate = default;

        var year = int.Parse(pesel[..2]);
        var month = int.Parse(pesel.Substring(2, 2));
        var day = int.Parse(pesel.Substring(4, 2));

        var century = month switch
        {
            >= 1 and <= 12 => 1900,
            >= 21 and <= 32 => 2000,
            >= 41 and <= 52 => 2100,
            >= 61 and <= 72 => 2200,
            >= 81 and <= 92 => 1800,
            _ => -1
        };

        if (century < 0)
            return false;

        var normalizedMonth = month % 20;
        var fullYear = century + year;

        return DateOnly.TryParse($"{fullYear:D4}-{normalizedMonth:D2}-{day:D2}", out birthDate);
    }
}
