using Lab3_5.BLL.EntityServices;
using Lab3_5.DAL;
using Lab3_5.DAL.Entities;
using Moq;

namespace Lab3_5.UnitTests;

public class SoftwareDeveloperServiceTests
{
    private readonly Mock<IEntityContext<SoftwareDeveloperEntity>> _mockContext;
    private readonly SoftwareDeveloperService _service;

    public SoftwareDeveloperServiceTests()
    {
        _mockContext = new Mock<IEntityContext<SoftwareDeveloperEntity>>();
        _service = new SoftwareDeveloperService(_mockContext.Object);
        _mockContext.Setup(c => c.Load()).Returns([]);
    }

    [Fact]
    public void GetAll_WhenEntitiesNotExist_ReturnsEmpty()
    {
        // Arrange
        //Initialized in constructor

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}