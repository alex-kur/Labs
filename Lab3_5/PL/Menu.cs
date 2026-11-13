using Lab3_5.BLL.EntityServices;
using Lab3_5.BLL.Models;
using Lab3_5.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Lab3_5.PL;

public sealed class Menu(IAppSettings appSettings, IServiceProvider serviceProvider) : IMenu
{
    public void MainMenu()
    {
        ConfigureSettings();

        while (true)
        {
            Console.Clear();
            PrintSettings();
            Console.WriteLine("1. Students");
            Console.WriteLine("2. Librarians");
            Console.WriteLine("3. Software developers");

            Console.WriteLine("4. Change settings");
            Console.WriteLine("0. Exit");
            Console.Write("Select action: ");

            var action = Console.ReadLine();

            try
            {
                switch (action)
                {
                    case "1": StudentsMenu(); break;
                    case "2": LibrariansMenu(); break;
                    case "3": SoftwareDevelopersMenu(); break;
                    case "4": ConfigureSettings(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid action."); break;
                }
            }
            catch (UnknownFileTypeException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Invalid file format]: {ex.Message}");
                Console.ResetColor();
            }
            catch (EntityValidationException ex)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[Validation error]: {ex.Message}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[Critical error]: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void ConfigureSettings()
    {
        Console.Clear();
        appSettings.FileType = string.Empty;
        Console.Write("Input data folder path: ");
        var inputPath = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(inputPath))
            appSettings.DataFolderPath = inputPath;

        Console.Clear();

        while (true)
        {
            Console.WriteLine("Select serialization format:");
            Console.WriteLine("1. JSON");
            Console.WriteLine("2. XML");
            Console.Write("Your choice: ");

            appSettings.FileType = Console.ReadLine() switch
            {
                "1" => "json",
                "2" => "xml",
                _ => string.Empty
            };

            if (string.IsNullOrWhiteSpace(appSettings.FileType))
            {
                Console.Clear();
                Console.WriteLine("Invalid choice. Try again.");
            }
            else
                break;
        }
    }

    private void StudentsMenu()
    {
        IStudentService studentService = serviceProvider.GetRequiredService<IStudentService>();

        while (true)
        {
            Console.Clear();
            PrintSettings();
            Console.WriteLine("===== Students =====");

            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Show at");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Remove");
            Console.WriteLine("5. Show with healthy weight");
            Console.WriteLine("0. Back");

            Console.Write("Select action: ");
            var action = Console.ReadLine();

            switch (action)
            {
                case "1": ShowAll(studentService); break;
                case "2": ShowAt(studentService); break;
                case "3": AddStudent(studentService); break;
                case "4": RemoveAt(studentService); break;
                case "5": ShowStudentsWithHealthyWeight(studentService); break;
                case "0": return;
                default: Console.WriteLine("Invalid action."); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void LibrariansMenu()
    {
        ILibrarianService librarianService = serviceProvider.GetRequiredService<ILibrarianService>();

        while (true)
        {
            Console.Clear();
            PrintSettings();
            Console.WriteLine("===== Librarians =====");

            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Show at");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Remove");
            Console.WriteLine("0. Back");

            Console.Write("Select action: ");
            var action = Console.ReadLine();

            switch (action)
            {
                case "1": ShowAll(librarianService); break;
                case "2": ShowAt(librarianService); break;
                case "3": AddLibrarian(librarianService); break;
                case "4": RemoveAt(librarianService); break;
                case "0": return;
                default: Console.WriteLine("Invalid action."); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void SoftwareDevelopersMenu()
    {
        ISoftwareDeveloperService softwareDeveloperService = serviceProvider.GetRequiredService<ISoftwareDeveloperService>();

        while (true)
        {
            Console.Clear();
            PrintSettings();
            Console.WriteLine("===== Software developers =====");

            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Show at");
            Console.WriteLine("3. Add");
            Console.WriteLine("4. Remove");
            Console.WriteLine("0. Back");

            Console.Write("Select action: ");
            var action = Console.ReadLine();

            switch (action)
            {
                case "1": ShowAll(softwareDeveloperService); break;
                case "2": ShowAt(softwareDeveloperService); break;
                case "3": AddSoftwareDeveloper(softwareDeveloperService); break;
                case "4": RemoveAt(softwareDeveloperService); break;
                case "0": return;
                default: Console.WriteLine("Invalid action."); break;
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private void ShowAll<TModel>(IEntityService<TModel> entityService) where TModel : Person
    {
        var persons = entityService.GetAll();

        if (persons.Count == 0)
        {
            Console.WriteLine("No data");
            return;
        }

        foreach (var (item, index) in persons.Select((value, i) => (value, i)))
            Console.WriteLine($"{index} {item.ToString()}");
    }

    private void ShowAt<TModel>(IEntityService<TModel> entityService) where TModel : Person
    {
        Console.Write("Input index: ");
        var index = int.Parse(Console.ReadLine()!);
        var persons = entityService.GetAll();
        Console.WriteLine(persons[index].ToString());
        Console.WriteLine("1. Make him ride a bike");
        Console.WriteLine("2. Make him lern online");
        Console.WriteLine("Any other to return");
        var answer = Console.ReadLine();

        switch (answer)
        {

            case "1": persons[index].RideBike(str => Console.WriteLine(str)); break;
            case "2": persons[index].LearnOnline(str => Console.WriteLine(str), InternetConnection.IsConnected()); break;
            default: break;
        }
    }

    private void RemoveAt<TModel>(IEntityService<TModel> entityService) where TModel : Person
    {
        Console.Write("Input index: ");
        var index = int.Parse(Console.ReadLine()!);
        entityService.RemoveAt(index);
        Console.WriteLine("Item removed");
    }

    private void AskPersonFields(out string firstName, out string lastName, out string passport)
    {
        Console.Write("FirstName: ");
        firstName = Console.ReadLine()!;
        Console.Write("LastName: ");
        lastName = Console.ReadLine()!;
        Console.Write("Passport in format AA000000: ");
        passport = Console.ReadLine()!;
    }

    private void AddStudent(IStudentService service)
    {
        AskPersonFields(out var firstName, out var lastName, out var passport);
        Console.Write("Hight: ");
        var height = int.Parse(Console.ReadLine()!);
        Console.Write("Weight: ");
        var weight = int.Parse(Console.ReadLine()!);
        Console.Write("Student ID card in format AA00000000: ");
        var studentIdCard = Console.ReadLine()!;

        var newStudent = new Student { FirstName = firstName, LastName = lastName, Passport = passport, Height = height, Weight = weight, StudentIdCard = studentIdCard };
        service.Add(newStudent);
        Console.WriteLine("Student added");
    }

    private void AddLibrarian(ILibrarianService service)
    {
        AskPersonFields(out var firstName, out var lastName, out var passport);
        Console.Write("Department: ");
        var department = Console.ReadLine()!;

        var newLibrarian = new Librarian { FirstName = firstName, LastName = lastName, Passport = passport, Department = department };
        service.Add(newLibrarian);
        Console.WriteLine("Librarian added");
    }

    private void AddSoftwareDeveloper(ISoftwareDeveloperService service)
    {
        AskPersonFields(out var firstName, out var lastName, out var passport);
        Console.Write("Experience: ");
        var experience = int.Parse(Console.ReadLine()!);
        Console.Write("Knowledge of programming languages ​​(separated by commas): ");
        var pgLanguages = Console.ReadLine()?.Split(",").Select(x => x.Trim()).ToList() ?? [];

        var newSoftwareDeveloper = new SoftwareDeveloper { FirstName = firstName, LastName = lastName, Passport = passport, Experience = experience, PgLanguages = pgLanguages };
        service.Add(newSoftwareDeveloper);
        Console.WriteLine("Software developer added");
    }

    private void ShowStudentsWithHealthyWeight(IStudentService service)
    {
        var students = service.GetStudentsWithHealthyWeight();

        if (students.Count == 0)
        {
            Console.WriteLine("No data");
            return;
        }

        foreach (var (item, index) in students.Select((value, i) => (value, i)))
            Console.WriteLine($"{index} {item.ToString()}");
    }

    private void PrintSettings()
    {
        Console.WriteLine($"===== DataFolderPath: {appSettings.DataFolderPath} | Serrialization type: {appSettings.DataFolderPath} =====");
    }
}
