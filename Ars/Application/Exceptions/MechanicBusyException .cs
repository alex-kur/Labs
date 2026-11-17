namespace Ars.Application.Exceptions;

public class MechanicBusyException : Exception
{
    public string FullName { get; }
    public MechanicBusyException(string fullName) : base($"Mechanic {fullName} is busy now")
    {
        FullName = fullName;
    }
}
