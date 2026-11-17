using Ars.Core;
using Ars.Domain.Entities;

namespace Ars.Tests;

public class CarTests
{
    [Theory]
    [InlineData(null, "A4", "AA1234BB")]
    [InlineData("", "A4", "AA1234BB")]
    [InlineData("   ", "A4", "AA1234BB")]
    [InlineData("Audi", null, "AA1234BB")]
    [InlineData("Audi", "", "AA1234BB")]
    [InlineData("Audi", "   ", "AA1234BB")]
    [InlineData("Audi", "A4", "A1234BB")]
    [InlineData("Audi", "A4", "AAA1234BB")]
    [InlineData("Audi", "A4", "AA123BB")]
    [InlineData("Audi", "A4", "AA12345BB")]
    [InlineData("Audi", "A4", "AA1234B")]
    [InlineData("Audi", "A4", "AA1234BBB")]
    public void CreateCar_InvalidValues_ThrowsValidationException(string? brand, string? model, string? licensePlate)
    {
        var ex = Assert.Throws<ValidationException>(() => Car.Create(brand!, model!, licensePlate!));
    }

    [Fact]
    public void ToString_ReturnsExpectedFormattedString()
    {
        var car = Car.Create("Audi", "A4", "AA1234BB");
        car.Id = 1;
        var excepted = "Id: 1, Audi A4 AA1234BB";
        Assert.Equal(excepted, car.ToString());
    }
}
