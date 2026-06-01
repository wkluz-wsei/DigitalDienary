using System.Text.RegularExpressions;

namespace CoreApp.Domain.ValueObjects;

public record PESEL
{
    public string Value { get; init; }

    public PESEL(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Invalid PESEL format or checksum.", nameof(value));

        Value = value;
    }

    public static bool IsValid(string pesel)
    {
        if (string.IsNullOrWhiteSpace(pesel) || pesel.Length != 11 || !Regex.IsMatch(pesel, @"^\d{11}$"))
            return false;

        int[] weights = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };
        int sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += (pesel[i] - '0') * weights[i];
        }

        int checksum = (10 - (sum % 10)) % 10;
        return checksum == (pesel[10] - '0');
    }

    public override string ToString() => Value;

    public static implicit operator string(PESEL pesel) => pesel.Value;
    public static explicit operator PESEL(string value) => new(value);
}
