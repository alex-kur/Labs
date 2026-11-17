using Ars.Domain.Entities;

namespace Ars.Data;

public class AutoRepairShopStorageModel
{
    public List<Car> Cars { get; set; } = new();
    public List<Mechanic> Mechanics { get; set; } = new();
    public List<SparePart> SpareParts { get; set; } = new();
    public List<RepairOrder> RepairOrders { get; set; } = new();
}
