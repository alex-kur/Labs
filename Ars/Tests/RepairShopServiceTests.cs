using Ars.Application.Exceptions;
using Ars.Application.Interfaces;
using Ars.Application.Services;
using Ars.Domain.Constants;
using Ars.Domain.Entities;
using Moq;
using System.Timers;

namespace Ars.Tests;

public class RepairShopServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<Car>> _carsRepoMock;
    private readonly Mock<IRepository<Mechanic>> _mechanicsRepoMock;
    private readonly Mock<IRepository<SparePart>> _sparePartsRepoMock;
    private readonly Mock<IRepository<RepairOrder>> _ordersRepoMock;
    private readonly RepairShopService _service;

    public RepairShopServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _carsRepoMock = new Mock<IRepository<Car>>();
        _mechanicsRepoMock = new Mock<IRepository<Mechanic>>();
        _sparePartsRepoMock = new Mock<IRepository<SparePart>>();
        _ordersRepoMock = new Mock<IRepository<RepairOrder>>();

        _unitOfWorkMock.Setup(u => u.Cars).Returns(_carsRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.Mechanics).Returns(_mechanicsRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.SpareParts).Returns(_sparePartsRepoMock.Object);
        _unitOfWorkMock.Setup(u => u.RepairOrders).Returns(_ordersRepoMock.Object);

        _service = new RepairShopService(_unitOfWorkMock.Object);
    }

    [Fact]
    public void GetAllMethods_ShouldReturnDataFromRepositories()
    {
        // Arrange
        var cars = new List<Car> { Car.Create("Toyota", "Camry", "AA1234BB") };
        var mechanics = new List<Mechanic> { Mechanic.Create("Ivan", "Petrenko", 15) };
        var parts = new List<SparePart> { SparePart.Create("Oil filter", 19m, 10) };
        var orders = new List<RepairOrder> { RepairOrder.Create(cars[0]) };

        _carsRepoMock.Setup(r => r.GetAll()).Returns(cars);
        _mechanicsRepoMock.Setup(r => r.GetAll()).Returns(mechanics);
        _sparePartsRepoMock.Setup(r => r.GetAll()).Returns(parts);
        _ordersRepoMock.Setup(r => r.GetAll()).Returns(orders);

        // Act & Assert
        Assert.Equal(cars, _service.GetAllCars());
        Assert.Equal(mechanics, _service.GetAllMechanics());
        Assert.Equal(parts, _service.GetAllSpareParts());
        Assert.Equal(orders, _service.GetAllOrders());
    }

    [Fact]
    public void GetRepairOrder_WhenOrderExists_ReturnsOrder()
    {
        // Arrange
        var car = Car.Create("BMW", "X5", "BC2222BC");
        var order = RepairOrder.Create(car);
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);

        // Act
        var result = _service.GetRepairOrder(1);

        // Assert
        Assert.Equal(order, result);
    }

    [Fact]
    public void GetRepairOrder_WhenOrderDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _ordersRepoMock.Setup(r => r.GetById(99)).Returns((RepairOrder?)null);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _service.GetRepairOrder(99));
    }

    [Fact]
    public void RegisterCar_WhenLicensePlateIsUnique_AddsCarToRepository()
    {
        // Arrange
        _carsRepoMock.Setup(r => r.GetAll()).Returns(new List<Car>());

        // Act
        _service.RegisterCar("Audi", "A4", "KA3333KA");

        // Assert
        _carsRepoMock.Verify(r => r.Add(It.Is<Car>(c =>
            c.Brand == "Audi" &&
            c.Model == "A4" &&
            c.LicencePlate == "KA3333KA")));
    }

    [Fact]
    public void RegisterCar_WhenLicensePlateAlreadyExists_ThrowsAlreadyExistsException()
    {
        // Arrange
        var existingCar = Car.Create("Audi", "A4", "KA3333KA");
        _carsRepoMock.Setup(r => r.GetAll()).Returns(new List<Car> { existingCar });

        // Act & Assert
        Assert.Throws<AlreadyExistsException>(() => _service.RegisterCar("BMW", "M3", "KA3333KA"));
    }

    [Fact]
    public void HireMechanic_ShouldCreateAndAddMechanicToRepository()
    {
        // Act
        _service.HireMechanic("Oleg", "Kovalenko", 7);

        // Assert
        _mechanicsRepoMock.Verify(r => r.Add(It.Is<Mechanic>(m =>
            m.FirstName == "Oleg" &&
            m.LastName == "Kovalenko" &&
            m.Experience == 7)));
    }

    [Fact]
    public void AddSparePartToStock_WhenPartDoesNotExist_AddsNewPart()
    {
        // Arrange
        _sparePartsRepoMock.Setup(r => r.GetAll()).Returns(new List<SparePart>());

        // Act
        _service.AddSparePartToStock("Brake pads", 120m, 8);

        // Assert
        _sparePartsRepoMock.Verify(r => r.Add(It.Is<SparePart>(sp =>
            sp.Name == "Brake pads" &&
            sp.UnitPrice == 120m &&
            sp.StockQuantity == 8)));
    }

    [Fact]
    public void AddSparePartToStock_WhenPartExists_UpdatesPriceAndIncreasesQuantity()
    {
        // Arrange
        var existingPart = SparePart.Create("Brake pads", 100m, 6);
        _sparePartsRepoMock.Setup(r => r.GetAll()).Returns(new List<SparePart> { existingPart });

        // Act
        _service.AddSparePartToStock("Brake pads", 125m, 8);

        // Assert
        Assert.Equal(125m, existingPart.UnitPrice);
        Assert.Equal(14, existingPart.StockQuantity);
    }

    [Fact]
    public void CreateRepairOrder_WhenCarExists_CreatesAndAddsOrder()
    {
        // Arrange
        var car = Car.Create("Ford", "Focus", "AE4444AE");
        _carsRepoMock.Setup(r => r.GetAll()).Returns(new List<Car> { car });

        // Act
        var order = _service.CreateRepairOrder("AE4444AE");

        // Assert
        Assert.NotNull(order);
        Assert.Equal(car, order.Vehicle);
    }

    [Fact]
    public void CreateRepairOrder_WhenCarDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        _carsRepoMock.Setup(r => r.GetAll()).Returns(new List<Car>());

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _service.CreateRepairOrder("UNKNOWN"));
        _ordersRepoMock.Verify(r => r.Add(It.IsAny<RepairOrder>()), Times.Never);
    }

    [Fact]
    public void UseSparePartInOrder_WhenValid_DecreasesStockAndAddsPartToOrder()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("VW", "Golf", "AA0001AA"));
        var part = SparePart.Create("Spark plug", 30m, 10);

        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _sparePartsRepoMock.Setup(r => r.GetById(1)).Returns(part);

        // Act
        _service.UseSparePartInOrder(1, 1, 4);

        // Assert
        Assert.Equal(6, part.StockQuantity);
        Assert.Single(order.SpareParts);
        Assert.Equal(4, order.SpareParts.First().Quantity);
    }

    [Fact]
    public void UseSparePartInOrder_WhenPartAlreadyInOrder_IncreasesQuantityInOrder()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("VW", "Golf", "AA0001AA"));
        var part = SparePart.Create("Spark plug", 30m, 10);
        var existingOrderPart = RepairOrderSparePart.Create(part, 2);        
        order.SpareParts.Add(existingOrderPart);

        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _sparePartsRepoMock.Setup(r => r.GetById(part.Id)).Returns(part);

        // Act
        _service.UseSparePartInOrder(1, part.Id, 3);

        // Assert
        Assert.Equal(7, part.StockQuantity);
        Assert.Single(order.SpareParts);
        Assert.Equal(5, existingOrderPart.Quantity);
    }

    [Fact]
    public void UseSparePartInOrder_WhenOrderOrPartNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns((RepairOrder?)null);

        // Act & Assert (Order not found)
        Assert.Throws<NotFoundException>(() => _service.UseSparePartInOrder(1, 1, 1));

        // Arrange (Part not found)
        var order = RepairOrder.Create(Car.Create("VW", "Golf", "AA0001AA"));
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _sparePartsRepoMock.Setup(r => r.GetById(1)).Returns((SparePart?)null);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _service.UseSparePartInOrder(1, 1, 1));
    }

    [Fact]
    public void UseSparePartInOrder_WhenOrderIsDone_ThrowsInvalidOperationException()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("VW", "Golf", "AA0001AA"));
        order.Status = RepairOrderStatus.Done;
        var part = SparePart.Create("Spark plug", 50m, 10);

        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _sparePartsRepoMock.Setup(r => r.GetById(1)).Returns(part);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.UseSparePartInOrder(1, 1, 1));
    }

    [Fact]
    public void UseSparePartInOrder_WhenNotEnoughStock_ThrowsSparePartOutOfStockException()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("VW", "Golf", "AA0001AA"));
        var part = SparePart.Create("Spark plug", 500m, 2);

        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _sparePartsRepoMock.Setup(r => r.GetById(1)).Returns(part);

        // Act & Assert
        Assert.Throws<SparePartOutOfStockException>(() => _service.UseSparePartInOrder(1, 1, 5));
    }

    [Fact]
    public void AssignMechanic_WhenValid_AssignsMechanicAndUpdatesStatuses()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("Honda", "Civic", "AI5555AI"));
        var mechanic = Mechanic.Create("Taras", "Shevchenko", 10);
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _mechanicsRepoMock.Setup(r => r.GetById(1)).Returns(mechanic);

        // Act
        _service.AssignMechanic(1, 1);

        // Assert
        Assert.True(mechanic.IsBusy);
        Assert.Equal(mechanic, order.AssignedMechanic);
        Assert.Equal(RepairOrderStatus.InProgres, order.Status);
    }

    [Theory]
    [InlineData(RepairOrderStatus.InProgres)]
    [InlineData(RepairOrderStatus.Done)]
    public void AssignMechanic_WhenOrderStatusIsInvalid_ThrowsInvalidOperationException(RepairOrderStatus status)
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("Honda", "Civic", "AI5555AI"));
        order.Status = status;
        var mechanic = Mechanic.Create("Taras", "Shevchenko", 10);
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _mechanicsRepoMock.Setup(r => r.GetById(1)).Returns(mechanic);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.AssignMechanic(1, 1));
    }
    [Fact]
    public void AssignMechanic_WhenMechanicIsBusy_ThrowsMechanicBusyException()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("Honda", "Civic", "AI5555AI"));
        var mechanic = Mechanic.Create("Taras", "Shevchenko", 10);
        mechanic.IsBusy = true;
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _mechanicsRepoMock.Setup(r => r.GetById(1)).Returns(mechanic);

        // Act & Assert
        Assert.Throws<MechanicBusyException>(() => _service.AssignMechanic(1, 1));
    }

    
    [Fact]
    public void CompleteOrder_WhenMechanicAssigned_FreesMechanicAndCompletesOrder()
    {
        // Arrange
        var order = RepairOrder.Create(Car.Create("Mazda", "6", "AX7777AX"));
        var mechanic = Mechanic.Create("Andrii", "Melnik", 4);
        mechanic.IsBusy = true;
        order.AssignedMechanic = mechanic;
        order.Status = RepairOrderStatus.InProgres;
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns(order);
        _mechanicsRepoMock.Setup(r => r.GetById(mechanic.Id)).Returns(mechanic);

        // Act
        _service.CompleteOrder(1);

        // Assert
        Assert.False(mechanic.IsBusy);
        Assert.Equal(RepairOrderStatus.Done, order.Status);
    }

    [Fact]
    public void CompleteOrder_WhenOrderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _ordersRepoMock.Setup(r => r.GetById(1)).Returns((RepairOrder?)null);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => _service.CompleteOrder(1));
    }
}
