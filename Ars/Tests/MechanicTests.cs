using Ars.Core;
using Ars.Domain.Entities;

namespace Ars.Tests;

public class MechanicTests
{
    [Theory]
    [InlineData(null, "Kovalenko", 10)]
    [InlineData("", "Kovalenko", 10)]
    [InlineData("   ", "Kovalenko", 10)]
    [InlineData("Artem", null, 10)]
    [InlineData("Artem", "", 10)]
    [InlineData("Artem", "   ", 10)]
    [InlineData("Artem", "Kovalenko", -5)]
    public void CreateMechanic_InvalidValues_ThrowsValidationException(string? firstName, string? lastName, int experience)
    {
        var ex = Assert.Throws<ValidationException>(() => Mechanic.Create(firstName!, lastName!, experience));
    }

    [Fact]
    public void ToString_ReturnsExpectedFormattedString()
    {
        var mechanic = Mechanic.Create("Artem", "Kovalenko", 10);
        mechanic.Id = 1;
        var excepted = "Id: 1, Artem Kovalenko, Experience: 10 years, Is busy: False";
        Assert.Equal(excepted, mechanic.ToString());
    }
}
