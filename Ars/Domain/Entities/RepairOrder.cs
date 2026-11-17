using Ars.Core;
using Ars.Domain.Constants;
using System.Text;

namespace Ars.Domain.Entities;

public class RepairOrder : IDomainEntity
{
    public static RepairOrder Create(Car vehicle)
    {
        return new() { Vehicle = vehicle };
    }

    public int Id { get; set; }

    public required Car Vehicle { get; set; }

    public List<RepairOrderSparePart> SpareParts { get; set; } = new();

    public Mechanic? AssignedMechanic { get; set; }

    public RepairOrderStatus Status { get; set; } = RepairOrderStatus.Pending;

    public decimal LaborCost
    {
        get => field;
        set
        {
            //Validate.IsGreaterThanZero(value);
            Validate.IsGreaterOrEqualZero(value);
            field = value;
        }
    }

    public decimal TotalCost => SpareParts.Sum(sp => sp.UnitPrice * sp.Quantity) + LaborCost;

    public string ToShortString()
    {
        return $"Repair order of car {Vehicle.LicencePlate}. Status: {Status}, Total cost: {TotalCost}";
    }

    public string ToDetailedString()
    {
        var sb = new StringBuilder();

        sb.AppendLine($"Repair order Id: {Id}, Status: {Status}, TotalCost: {TotalCost}");
        sb.AppendLine($"Car: {Vehicle.ToString()}");

        if (AssignedMechanic != null)
            sb.AppendLine($"Responsible mechanic: {AssignedMechanic.ToString()}");

        if(SpareParts.Any())
        {
            sb.AppendLine("Spare parts:");

            foreach (var part in SpareParts)
                sb.AppendLine($"\t-{part.ToString()}");
        }

        if (LaborCost != 0m)
            sb.AppendLine($"Labor Cost: {LaborCost:0.##}$");

        return sb.ToString();
    }
}
