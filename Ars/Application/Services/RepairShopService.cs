using Ars.Application.Exceptions;
using Ars.Application.Interfaces;
using Ars.Core;
using Ars.Domain.Constants;
using Ars.Domain.Entities;

namespace Ars.Application.Services;

public class RepairShopService(IUnitOfWork _unitOfWork) : IRepairShopService
{
    public IReadOnlyList<Car> GetAllCars() => _unitOfWork.Cars.GetAll();
    public IReadOnlyList<Mechanic> GetAllMechanics() => _unitOfWork.Mechanics.GetAll();
    public IReadOnlyList<SparePart> GetAllSpareParts() => _unitOfWork.SpareParts.GetAll();
    public IReadOnlyList<RepairOrder> GetAllOrders() => _unitOfWork.RepairOrders.GetAll();

    public RepairOrder GetRepairOrder(int id)
    {
        var order = _unitOfWork.RepairOrders.GetById(id);

        if (order == null)
            throw new NotFoundException($"Order with Id: {id} not found.");

        return order;
    }

    public void RegisterCar(string brand, string model, string licensePlate)
    {
        if (_unitOfWork.Cars.GetAll().Any(c => c.LicencePlate == licensePlate))
            throw new AlreadyExistsException($"Car {licensePlate} already exists.");

        var car = Car.Create(brand, model, licensePlate);
        _unitOfWork.Cars.Add(car);
    }

    public void HireMechanic(string firstName, string lastName, int experience)
    {
        var mechanic = Mechanic.Create(firstName, lastName, experience);
        _unitOfWork.Mechanics.Add(mechanic);
    }

    public void AddSparePartToStock(string name, decimal unitPrice, int quantity)
    {
        var sparePart = _unitOfWork.SpareParts.GetAll().FirstOrDefault(sp => sp.Name == name);

        if (sparePart != null)
        {
            sparePart.UnitPrice = unitPrice;
            sparePart.StockQuantity += quantity;
        }
        else
        {
            sparePart = SparePart.Create(name, unitPrice, quantity);
            _unitOfWork.SpareParts.Add(sparePart);
        }
    }

    public RepairOrder CreateRepairOrder(string carLicencePlate)
    {
        var car = _unitOfWork.Cars.GetAll().FirstOrDefault(c => c.LicencePlate == carLicencePlate);

        if (car == null)
            throw new NotFoundException($"Car with licence plate {carLicencePlate} does not exists.");

        var order = RepairOrder.Create(car);
        _unitOfWork.RepairOrders.Add(order);
        return order;
    }

    public void UseSparePartInOrder(int orderId, int partId, int quantity)
    {
        var order = _unitOfWork.RepairOrders.GetById(orderId) ?? throw new NotFoundException($"Order with Id: {orderId} not found.");
        var sparePart = _unitOfWork.SpareParts.GetById(partId) ?? throw new NotFoundException($"Spare part with Id: {partId} not found.");
        Validate.IsGreaterThanZero(quantity);

        if (order.Status == RepairOrderStatus.Done)
            throw new InvalidOperationException("Can't add spare part to complete order.");

        if (sparePart.StockQuantity < quantity)
            throw new SparePartOutOfStockException(sparePart.Name, quantity, sparePart.StockQuantity);

        sparePart.StockQuantity -= quantity;

        var orderSparePart = order.SpareParts.FirstOrDefault(sp => sp.SparePartId == partId);

        if (orderSparePart != null)
            orderSparePart.Quantity += quantity;
        else
            order.SpareParts.Add(RepairOrderSparePart.Create(sparePart, quantity));
    }

    public void AssignMechanic(int orderId, int mechanicId)
    {
        var order = _unitOfWork.RepairOrders.GetById(orderId) ?? throw new NotFoundException($"Order with Id: {orderId} not found.");
        var mechanic = _unitOfWork.Mechanics.GetById(mechanicId) ?? throw new NotFoundException($"Mechanic with Id: {mechanicId} not found.");

        if (order.Status == RepairOrderStatus.InProgres)
            throw new InvalidOperationException($"Mechanic allready assigned to order {orderId}.");

        if (order.Status == RepairOrderStatus.Done)
            throw new InvalidOperationException($"Can't assign mechanic to completed order.");

        if (mechanic.IsBusy)
            throw new MechanicBusyException(mechanic.FullName);

        mechanic.IsBusy = true;
        order.AssignedMechanic = mechanic;
        order.Status = RepairOrderStatus.InProgres;
    }

    public void CompleteOrder(int orderId)
    {
        var order = _unitOfWork.RepairOrders.GetById(orderId) ?? throw new NotFoundException($"Order with Id: {orderId} not found.");

        if (order.AssignedMechanic != null)
        {
            var mechanic = _unitOfWork.Mechanics.GetById(order.AssignedMechanic.Id);

            if (mechanic != null)
                mechanic.IsBusy = false;

            order.AssignedMechanic.IsBusy = false;
        }

        order.Status = RepairOrderStatus.Done;
    }

    public void Initialize() => _unitOfWork.LoadData();
    public void SaveAllData() => _unitOfWork.SaveChanges();
}
