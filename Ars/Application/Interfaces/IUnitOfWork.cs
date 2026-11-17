using Ars.Domain.Entities;

namespace Ars.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<Car> Cars { get; }
    IRepository<Mechanic> Mechanics { get; }
    IRepository<SparePart> SpareParts { get; }
    IRepository<RepairOrder> RepairOrders { get; }

    void SaveChanges();
    void LoadData();
}
