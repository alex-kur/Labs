using Ars.Core;

namespace Ars.Domain.Entities;

public class Mechanic : IDomainEntity
{
    public static Mechanic Create(string firstName, string lastName, int experience)
    {
        return new()
        {
            FirstName = firstName,
            LastName = lastName,
            Experience = experience
        };
    }

    public int Id { get; set; }

    public required string FirstName
    {
        get => field;
        set
        {
            Validate.IsNotNullOrWhiteSpace(value);
            field = value;
        }
    }

    public required string LastName
    {
        get => field;
        set
        {
            Validate.IsNotNullOrWhiteSpace(value);
            field = value;
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Years
    /// </summary>
    public required int Experience
    {
        get => field;
        set
        {
            Validate.IsGreaterOrEqualZero(value);
            field = value;
        }
    }

    public bool IsBusy { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}, {FullName}, Experience: {Experience} years, Is busy: {IsBusy}";
    }
}
