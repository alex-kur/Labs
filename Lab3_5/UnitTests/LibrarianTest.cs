using Lab3_5.BLL.Models;
using Lab3_5.Core;

namespace Lab3_5.UnitTests;

public class LibrarianTest
{
    private static Librarian CreateValidLibrarian(
        string firstName = "Maria",
        string lastName = "Kovalenko",
        string passport = "BH502842",
        string department = "Shevchenkivskyi")
    {
        return new()
        {
            FirstName = firstName,
            LastName = lastName,
            Passport = passport,
            Department = department
        };
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Department_InvalidValue_ThrowsEntityValidationException(string? invalidDepartment)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidLibrarian(department: invalidDepartment!));

        Assert.Equal("Department can't be empty", ex.Message);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormattedString()
    {
        var librarian = CreateValidLibrarian();
        var expected = "Librarian Maria Kovalenko. Passport: BH502842, Department: Shevchenkivskyi";

        Assert.Equal(expected, librarian.ToString());
    }
}
