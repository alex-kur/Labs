using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Ars.Core;

public static class Validate
{
    private static readonly string _licencePlateNumberRegExp = "^[A-Z]{2}\\d{4}[A-Z]{2}$";

    public static void IsNotNullOrWhiteSpace(string value, [CallerMemberName] string propertyName = "")
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException($"{propertyName} can't be empty");
    }

    public static void IsGreaterOrEqualZero(int value, [CallerMemberName] string propertyName = "")
    {
        if (value < 0)
            throw new ValidationException($"{propertyName} can't be less than 0");
    }

    public static void IsGreaterThanZero(decimal value, [CallerMemberName] string propertyName = "")
    {
        if (value <= 0)
            throw new ValidationException($"{propertyName} must be greater than 0");
    }

    public static void IsGreaterOrEqualZero(decimal value, [CallerMemberName] string propertyName = "")
    {
        if (value < 0)
            throw new ValidationException($"{propertyName} can't be less than 0");
    }

    public static void IsLicencePlateNumber(string value, [CallerMemberName] string propertyName = "")
    {
        if (!Regex.IsMatch(value, _licencePlateNumberRegExp))
            throw new ValidationException("Licence plate must be in the format AA0000AA");
    }
}
