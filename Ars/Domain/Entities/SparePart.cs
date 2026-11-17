using Ars.Core;

namespace Ars.Domain.Entities;

public class SparePart : IDomainEntity, IEquatable<SparePart>
{
    public static SparePart Create(string name, decimal unitPrice, int stockQuantity)
    {
        return new()
        {
            Name = name,
            UnitPrice = unitPrice,
            StockQuantity = stockQuantity
        };
    }

    public int Id { get; set; }

    public required string Name
    {
        get => field;
        set
        {
            Validate.IsNotNullOrWhiteSpace(value);
            field = value;
        }
    }

    public required decimal UnitPrice
    {
        get => field;
        set
        {
            Validate.IsGreaterThanZero(value);
            field = value;
        }
    }

    public required int StockQuantity
    {
        get => field;
        set
        {
            Validate.IsGreaterOrEqualZero(value);
            field = value;
        }
    }

    public bool Equals(SparePart? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return Name == other.Name;
    }

    public override string ToString() => $"Id: {Id}. {Name} Price: {UnitPrice:0.##}$ Stock quantity: {StockQuantity}";
}
