using Lab3_5.BLL.EntityServices;
using Lab3_5.BLL.Models;
using Lab3_5.DAL;
using Lab3_5.DAL.Entities;
using Moq;

namespace Lab3_5.UnitTests;

public class StudentServiceTests
{
    private readonly Mock<IEntityContext<StudentEntity>> _mockContext;
    private readonly StudentService _service;
    private readonly List<StudentEntity> _initialStudents = new()
    {
        new (){ FirstName = "Olena", LastName="Shevchenko", Passport="AB123456", Height=170, Weight = 60, StudentIdCard = "CD45127834" },
        new (){ FirstName = "Ivan", LastName="Petrenko", Passport="EF762836", Height=175, Weight = 90, StudentIdCard = "JH48253284" }
    };

    public StudentServiceTests()
    {
        _mockContext = new Mock<IEntityContext<StudentEntity>>();
        _service = new StudentService(_mockContext.Object);
        _mockContext.Setup(c => c.Load()).Returns(_initialStudents);
    }

    [Fact]
    public void GetAll_WhenEntitiesExist_ReturnsModels()
    {
        // Arrange
        //Initialized in constructor

        // Act
        var result = _service.GetAll();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void GetAt_ValidIndex_ReturnsModel()
    {
        // Arrange
        //Initialized in constructor

        // Act
        var result = _service.GetAt(1);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetAt_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        //Initialized in constructor

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.GetAt(5));
    }

    [Fact]
    public void Add_ValidStudent_AddsEntityAndSavesContext()
    {
        // Arrange
        //Initialized in constructor

        var newStudent = new Student
        {
            FirstName = "Maria",
            LastName = "Kovalenko",
            Passport = "BH502842",
            Height = 183,
            Weight = 70,
            StudentIdCard = "JH48253284"
        };

        // Act
        _service.Add(newStudent);

        // Assert
        Assert.Equal(3, _initialStudents.Count);
        Assert.Contains(_initialStudents, s => s.FirstName == "Maria");
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesEntityAndSavesContext()
    {
        // Arrange
        //Initialized in constructor

        // Act
        _service.RemoveAt(1);

        // Assert
        Assert.Single(_initialStudents);
        Assert.DoesNotContain(_initialStudents, s => s.FirstName == "Ivan");
    }

    [Fact]
    public void RemoveAt_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        //Initialized in constructor

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => _service.RemoveAt(6));
    }

    [Fact]
    public void GetStudentsWithHealthyWeight()
    {
        // Arrange
        //Initialized in constructor

        // Act
        var result = _service.GetStudentsWithHealthyWeight();

        // Assert
        Assert.Single(result);
        Assert.Equal("Olena", result[0].FirstName);
    }
}