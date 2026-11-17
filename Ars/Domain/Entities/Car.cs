using Ars.Core;

namespace Ars.Domain.Entities;

public class Car : IDomainEntity, IEquatable<Car>
{
    public static Car Create(string brand, string model, string licensePlate)
    {
        return new()
        {
            Brand = brand,
            Model = model,
            LicencePlate = licensePlate
        };
    }

    public int Id { get; set; }

    public required string Brand
    {
        get => field;
        set
        {
            Validate.IsNotNullOrWhiteSpace(value);
            field = value;
        }
    }

    public required string Model
    {
        get => field;
        set
        {
            Validate.IsNotNullOrWhiteSpace(value);
            field = value;
        }
    }

    /// <summary>
    /// AA0000AA
    /// </summary>
    public required string LicencePlate
    {
        get => field;
        set
        {
            Validate.IsNotNullOrWhiteSpace(value);
            Validate.IsLicencePlateNumber(value);
            field = value;
        }
    }

    public bool Equals(Car? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return LicencePlate == other.LicencePlate;
    }

    public override bool Equals(object? obj) => Equals(obj as Car);

    public override int GetHashCode() => LicencePlate.GetHashCode();

    public override string ToString() => $"Id: {Id}, {Brand} {Model} {LicencePlate}";
}
