using Ars.Application.Services;
using System.CommandLine;

namespace Ars.Cli;

public class Menu(IRepairShopService _repairShopService) : IMenu
{
    private readonly Option<int> _orderIdOption = new("--order-id") { Description = "Repair order id" };
    private readonly Option<int> _mechanicIdOption = new("--mechanic-id") { Description = "Mechanic id" };
    private readonly Option<int> _sparePartIdOption = new("--part-id") { Description = "Spare part id" };

    public RootCommand Create()
    {
        var rootCommand = new RootCommand("Auto Repair Shop CLI");
        AddHireMechanicCommand(rootCommand);
        AddPrintAllMechanicsCommand(rootCommand);
        AddAddSparePartToStockCommand(rootCommand);
        AddPrintAllSparePartsCommand(rootCommand);
        AddRegisterCarCommand(rootCommand);
        AddPrintAllCarsCommand(rootCommand);
        AddCreateRepairOrderCommand(rootCommand);
        AddUseSparePartInOrderCommand(rootCommand);
        AddAssignMechanicCommand(rootCommand);
        AddCompleteOrderCommand(rootCommand);
        AddPrintAllRepairOrdersCommand(rootCommand);
        AddPrintRepairOrderCommand(rootCommand);

        return rootCommand;
    }

    private void AddPrintAllMechanicsCommand(RootCommand rootCommand)
    {
        var command = new Command("print-mechanics", "Print all available mechanics");
        command.SetAction(pr =>
        {
            var mechanics = _repairShopService.GetAllMechanics();

            if (!mechanics.Any())
            {
                Console.WriteLine("No data.");
                return;
            }

            foreach (var mech in mechanics)
                Console.WriteLine(mech.ToString());

            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddPrintAllCarsCommand(RootCommand rootCommand)
    {
        var command = new Command("print-cars", "Print all registered cars");
        command.SetAction(pr =>
        {
            var cars = _repairShopService.GetAllCars();

            if (!cars.Any())
            {
                Console.WriteLine("No data.");
                return;
            }

            foreach (var car in cars)
                Console.WriteLine(car.ToString());

            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddPrintAllRepairOrdersCommand(RootCommand rootCommand)
    {
        var command = new Command("print-orders", "Print all repair orders");
        command.SetAction(pr =>
        {
            var orders = _repairShopService.GetAllOrders();

            if (!orders.Any())
            {
                Console.WriteLine("No data.");
                return;
            }

            foreach (var order in orders)
                Console.WriteLine(order.ToShortString());

            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddPrintAllSparePartsCommand(RootCommand rootCommand)
    {
        var command = new Command("print-spare-parts", "Print all spare parts");
        command.SetAction(pr =>
        {
            var spareParts = _repairShopService.GetAllSpareParts();

            if (!spareParts.Any())
            {
                Console.WriteLine("No data.");
                return;
            }

            foreach (var part in spareParts)
                Console.WriteLine(part.ToString());

            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddAssignMechanicCommand(RootCommand rootCommand)
    {
        var command = new Command("assign-mechanic", "Assign mechanic to the repair order")
        {
            Options = { _orderIdOption, _mechanicIdOption }
        };

        command.SetAction(pr =>
        {
            var orderId = pr.GetValue(_orderIdOption);
            var mechanicId = pr.GetValue(_mechanicIdOption);

            _repairShopService.AssignMechanic(orderId, mechanicId);
            Console.WriteLine("Mechanic assigned to repair order.");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddCompleteOrderCommand(RootCommand rootCommand)
    {
        var command = new Command("complete-order", "Complete order")
        {
            Options = { _orderIdOption }
        };

        command.SetAction(pr =>
        {
            var orderId = pr.GetValue(_orderIdOption);

            _repairShopService.CompleteOrder(orderId);
            Console.WriteLine("Order completed.");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddPrintRepairOrderCommand(RootCommand rootCommand)
    {
        var command = new Command("print-order", "Print repair order details")
        {
            Options = { _orderIdOption }
        };

        command.SetAction(pr =>
        {
            var orderId = pr.GetValue(_orderIdOption);

            var repairOrder = _repairShopService.GetRepairOrder(orderId);
            Console.WriteLine(repairOrder.ToDetailedString());
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddUseSparePartInOrderCommand(RootCommand rootCommand)
    {
        var quantityOption = new Option<int>("--quantity") { Description = "Quantity" };

        var command = new Command("use-spare-part", "Add spare part to the repair order")
        {
            Options = { _orderIdOption, _sparePartIdOption, quantityOption }
        };

        command.SetAction(pr =>
        {
            var orderId = pr.GetValue(_orderIdOption);
            var partId = pr.GetValue(_sparePartIdOption);
            var quantity = pr.GetValue(quantityOption);

            _repairShopService.UseSparePartInOrder(orderId, partId, quantity);
            Console.WriteLine("Spare part added to the repair order.");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddCreateRepairOrderCommand(RootCommand rootCommand)
    {
        var licensePlateOption = new Option<string>("--car-license-plate") { Description = "Car license plate" };

        var command = new Command("create-repair-order", "Creates a repair order for car")
        {
            Options = { licensePlateOption }
        };

        command.SetAction(pr =>
        {
            var licensePlate = pr.GetValue(licensePlateOption);
            var order = _repairShopService.CreateRepairOrder(licensePlate!);
            Console.WriteLine($"Repair order created. Id: {order.Id}");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    public void AddAddSparePartToStockCommand(RootCommand rootCommand)
    {
        var nameOption = new Option<string>("--name") { Description = "Spare part name" };
        var unitPriceOption = new Option<Decimal>("--price") { Description = "Price for one unit" };
        var quantityOption = new Option<int>("--quantity") { Description = "Quantity" };

        var command = new Command("spare-part-arrival", "Add spare parts to stock")
        {
            Options = { nameOption, unitPriceOption, quantityOption }
        };

        command.SetAction(pr =>
        {
            var name = pr.GetValue(nameOption);
            var unitPrice = pr.GetValue(unitPriceOption);
            var quantity = pr.GetValue(quantityOption);
            _repairShopService.AddSparePartToStock(name!, unitPrice, quantity);
            Console.WriteLine($"Spare part added to stock");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddHireMechanicCommand(RootCommand rootCommand)
    {
        var firstNameOption = new Option<string>("--first-name") { Description = "First name" };
        var lastNameOption = new Option<string>("--last-name") { Description = "Last name" };
        var experienceOption = new Option<int>("--experience") { Description = "Experience in years" };

        var command = new Command("hire-mechanic", "Hire a new mechanic")
        {
            Options = { firstNameOption, lastNameOption, experienceOption }
        };

        command.SetAction(pr =>
        {
            var firstName = pr.GetValue(firstNameOption);
            var lastName = pr.GetValue(lastNameOption);
            var experience = pr.GetValue(experienceOption);
            _repairShopService.HireMechanic(firstName!, lastName!, experience);
            Console.WriteLine($"Mechanic haried");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }

    private void AddRegisterCarCommand(RootCommand rootCommand)
    {
        var brandOption = new Option<string>("--brand") { Description = "Car brand" };
        var modelOption = new Option<string>("--model") { Description = "Car model" };
        var licensePlateOption = new Option<string>("--license-plate") { Description = "License plate (in format AA0000AA)" };

        var command = new Command("register-car", "Register car")
        {
            Options = { brandOption, modelOption, licensePlateOption }
        };

        command.SetAction(pr =>
        {
            var brand = pr.GetValue(brandOption);
            var model = pr.GetValue(modelOption);
            var licensePlate = pr.GetValue(licensePlateOption);
            _repairShopService.RegisterCar(brand!, model!, licensePlate!);
            Console.WriteLine($"Car registerd");
            Console.WriteLine();
        });

        rootCommand.Subcommands.Add(command);
    }
}
