using Lab3_5.Core;

namespace Lab3_5.BLL.Models;

public class SoftwareDeveloper : Person
{
    /// <summary>
    /// Years
    /// </summary>
    public required int Experience
    {
        get => field;
        set
        {
            if (value < 0)
                throw new EntityValidationException("Experience can not be negative");

            field = value;
        }
    }

    /// <summary>
    /// Knowledge of programming languages
    /// </summary>
    public required List<string> PgLanguages { get; set; }

    protected override string ToStringImp() => $"Software developer {FullName}. Passport: {Passport}, Experience: {Experience} years, Knowledge of programming languages: {string.Join(", ", PgLanguages)}";
}
