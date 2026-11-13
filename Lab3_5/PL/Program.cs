using Lab3_5.BLL.EntityServices;
using Lab3_5.Core;
using Lab3_5.DAL;
using Lab3_5.DAL.DataProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Lab3_5.PL;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IMenu, Menu>()
            .AddSingleton<IAppSettings, AppSettings>()
            .AddTransient<IStudentService, StudentService>()
            .AddTransient<ILibrarianService, LibrarianService>()
            .AddTransient<ISoftwareDeveloperService, SoftwareDeveloperService>()
            .AddTransient(typeof(IEntityContext<>), typeof(EntityContext<>))
            .AddKeyedTransient(typeof(IDataProvider<>), "json", typeof(JsonDataProvider<>))
            .AddKeyedTransient(typeof(IDataProvider<>), "xml", typeof(XmlDataProvider<>))
            .BuildServiceProvider();

        var menu = serviceProvider.GetRequiredService<IMenu>();
        menu.MainMenu();
    }
}
