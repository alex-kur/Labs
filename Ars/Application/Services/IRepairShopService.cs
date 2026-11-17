using Ars.Domain.Entities;

namespace Ars.Application.Services
{
    public interface IRepairShopService
    {
        void AddSparePartToStock(string name, decimal unitPrice, int quantity);
        void AssignMechanic(int orderId, int mechanicId);
        void CompleteOrder(int orderId);
        RepairOrder CreateRepairOrder(string carLicencePlate);
        IReadOnlyList<Car> GetAllCars();
        IReadOnlyList<Mechanic> GetAllMechanics();
        IReadOnlyList<RepairOrder> GetAllOrders();
        IReadOnlyList<SparePart> GetAllSpareParts();
        RepairOrder GetRepairOrder(int id);
        void HireMechanic(string firstName, string lastName, int experience);
        void Initialize();
        void RegisterCar(string brand, string model, string licensePlate);
        void SaveAllData();
        void UseSparePartInOrder(int orderId, int partId, int quantity);
    }
}