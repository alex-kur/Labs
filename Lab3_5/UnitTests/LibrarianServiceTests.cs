using Lab3_5.BLL.EntityServices;
using Lab3_5.DAL;
using Lab3_5.DAL.Entities;
using Moq;

namespace Lab3_5.UnitTests;

public class LibrarianServiceTests
{
    private readonly Mock<IEntityContext<LibrarianEntity>> _mockContext;
    private readonly LibrarianService _service;

    public LibrarianServiceTests()
    {
        _mockContext = new Mock<IEntityContext<LibrarianEntity>>();
        _service = new LibrarianService(_mockContext.Object);
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