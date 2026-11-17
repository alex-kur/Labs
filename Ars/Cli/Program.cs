using Ars.Application.Interfaces;
using Ars.Application.Services;
using Ars.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Ars.Cli;

public class Program
{
    private static IServiceProvider _serviceProvider;

    public static void Main(string[] args)
    {
        _serviceProvider = InitializeDependencies();
        LoadData();
        AppDomain.CurrentDomain.ProcessExit += Application_OnProcessExit;
        Run();
    }

    private static void Run()
    {
        var menu = _serviceProvider.GetRequiredService<IMenu>();
        var rootCommand = menu.Create();

        Console.WriteLine("Welcome to Auto Repair Shop.\r\nType 'exit' to quit\r\nType '-h' to show features\r\nType a command:");

        while (true)
        {
            Console.Write("> ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
                continue;

            if (input.Trim().Equals("exit"))
                break;

            try
            {
                rootCommand.Parse(input).Invoke(new() { EnableDefaultExceptionHandler = false });
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
                Console.WriteLine();
            }
        }
    }

    private static ServiceProvider InitializeDependencies()
    {
        return new ServiceCollection()
            .AddTransient<IRepairShopService, RepairShopService>()
            .AddSingleton<IUnitOfWork, JsonUnitOfWork>()
            .AddSingleton<IMenu, Menu>()
            .AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>))
            .BuildServiceProvider();
    }

    private static void LoadData()
    {
        try
        {
            var repairShopService = _serviceProvider.GetRequiredService<IRepairShopService>();
            repairShopService.Initialize();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Error: Can't load data from file:");
            Console.WriteLine($"{ex.Message}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }

    private static void Application_OnProcessExit(object? sender, EventArgs args)
    {
        try
        {
            var repairShopService = _serviceProvider.GetRequiredService<IRepairShopService>();
            repairShopService.SaveAllData();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
