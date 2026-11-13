namespace Lab3_5.Core;

public class EntityValidationException : Exception
{
    public EntityValidationException(string message) : base(message) { }
}
