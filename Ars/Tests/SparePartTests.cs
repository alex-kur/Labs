using Ars.Core;
using Ars.Domain.Entities;
using System.Xml.Linq;

namespace Ars.Tests;

public class SparePartTests
{
    [Theory]
    [InlineData(null, 19, 5)]
    [InlineData("", 19, 5)]
    [InlineData("   ", 19, 5)]
    [InlineData("Oil filter", -11, 5)]
    [InlineData("Oil filter", 0, 5)]
    [InlineData("Oil filter", 19, -5)]
    public void CreateSparePart_InvalidValues_ThrowsValidationException(string? name, decimal unitPrice, int stockQuantity)
    {
        var ex = Assert.Throws<ValidationException>(() => SparePart.Create(name!, unitPrice, stockQuantity));
    }

    [Fact]
    public void ToString_ReturnsExpectedFormattedString()
    {
        var sparePart = SparePart.Create("Oil filter", 19, 5);
        sparePart.Id = 1;
        var excepted = "Id: 1. Oil filter Price: 19$ Stock quantity: 5";
        Assert.Equal(excepted, sparePart.ToString());
    }
}
