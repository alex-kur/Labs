using Ars.Application.Interfaces;
using Ars.Domain.Entities;
using System.Text.Json;

namespace Ars.Data;

public class JsonUnitOfWork : IUnitOfWork
{
    private const string _dataFileName = "AutoRepairShopData.json";

    public IRepository<Car> Cars { get; }
    public IRepository<Mechanic> Mechanics { get; }
    public IRepository<SparePart> SpareParts { get; }
    public IRepository<RepairOrder> RepairOrders { get; }

    public JsonUnitOfWork(IRepository<Car> carsRepo,
                          IRepository<Mechanic> mechanicsRepo,
                          IRepository<SparePart> sparePartsRepo,
                          IRepository<RepairOrder> ordersRepo)
    {
        Cars = carsRepo;
        Mechanics = mechanicsRepo;
        SpareParts = sparePartsRepo;
        RepairOrders = ordersRepo;
    }

    public void SaveChanges()
    {
        var storageModel = new AutoRepairShopStorageModel
        {
            Cars = Cars.GetAll().ToList(),
            Mechanics = Mechanics.GetAll().ToList(),
            SpareParts = SpareParts.GetAll().ToList(),
            RepairOrders = RepairOrders.GetAll().ToList()
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(storageModel, options);
        File.WriteAllText(_dataFileName, json);
    }

    public void LoadData()
    {
        if (!File.Exists(_dataFileName))
            return;

        string json = File.ReadAllText(_dataFileName);

        if (string.IsNullOrWhiteSpace(json))
            return;

        var storageModel = JsonSerializer.Deserialize<AutoRepairShopStorageModel>(json);

        if (storageModel != null)
        {
            Cars.Initialize(storageModel.Cars);
            Mechanics.Initialize(storageModel.Mechanics);
            SpareParts.Initialize(storageModel.SpareParts);
            RepairOrders.Initialize(storageModel.RepairOrders);
        }
    }
}
