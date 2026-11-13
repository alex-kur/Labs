using Lab3_5.Core;

namespace Lab3_5.BLL.Models;

public class Librarian : Person
{
    public required string Department
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityValidationException("Department can't be empty");

            field = value;
        }
    }

    protected override string ToStringImp() => $"Librarian {FullName}. Passport: {Passport}, Department: {Department}";
}
