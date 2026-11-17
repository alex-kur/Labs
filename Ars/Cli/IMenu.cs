using System.CommandLine;

namespace Ars.Cli;

public interface IMenu
{
    RootCommand Create();
}
