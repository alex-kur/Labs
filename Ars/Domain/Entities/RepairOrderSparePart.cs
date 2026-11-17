using Ars.Core;

namespace Ars.Domain.Entities;

public class RepairOrderSparePart
{
    public static RepairOrderSparePart Create(SparePart sparePart, int quantity)
    {
        return new()
        {
            SparePartId = sparePart.Id,
            SparePartName = sparePart.Name,
            UnitPrice = sparePart.UnitPrice,
            Quantity = quantity
        };
    }

    public int SparePartId { get; set; }

    public required string SparePartName
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

    public required int Quantity
    {
        get => field;
        set
        {
            Validate.IsGreaterThanZero(value);
            field = value;
        }
    }

    public override string ToString()
    {
        return $"Id: {SparePartId}, {SparePartName}, Unit price: {UnitPrice:0.##}$, Quantity: {Quantity}, Total price: {UnitPrice * Quantity:0.##}";
    }
}
