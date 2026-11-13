using Lab3_5.BLL.Models;
using Lab3_5.Core;

namespace Lab3_5.UnitTests;

public class StudentTests
{
    private static Student CreateValidStudent(
        string firstName = "Olena",
        string lastName = "Shevchenko",
        string passport = "AB123456",
        int height = 170,
        int weight = 60,
        string studentIdCard = "CD45127834")
    {
        return new()
        {
            FirstName = firstName,
            LastName = lastName,
            Passport = passport,
            Height = height,
            Weight = weight,
            StudentIdCard = studentIdCard
        };
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FirstName_InvalidValue_ThrowsEntityValidationException(string? invalidFirstName)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidStudent(firstName: invalidFirstName!));

        Assert.Equal("First name can't be empty", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void LastName_InvalidValue_ThrowsEntityValidationException(string? invalidLastName)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidStudent(lastName: invalidLastName!));

        Assert.Equal("Last name can't be empty", ex.Message);
    }

    [Theory]
    [InlineData("A123456")]
    [InlineData("AAA123456")]
    [InlineData("AA12345")]
    [InlineData("AA1234567")]
    [InlineData("12345678")]
    [InlineData("AA-123456")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void Passport_InvalidFormat_ThrowsEntityValidationException(string? invalidPassport)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidStudent(passport: invalidPassport!));
    }

    [Theory]
    [InlineData("AA123456")]
    [InlineData("yy987654")]
    public void Passport_ValidFormat_SetsProperty(string validPassport)
    {
        var student = CreateValidStudent(passport: validPassport);
        Assert.Equal(validPassport, student.Passport);
    }

    [Fact]
    public void FullName_ReturnsFormattedFirstAndLastName()
    {
        var student = CreateValidStudent(firstName: "Olena", lastName: "Shevchenko");
        Assert.Equal("Olena Shevchenko", student.FullName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-160)]
    public void Height_LessOrEqualZero_ThrowsEntityValidationException(int invalidHeight)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidStudent(height: invalidHeight));

        Assert.Equal("Height can't be less or equals 0", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-70)]
    public void Weight_LessOrEqualZero_ThrowsEntityValidationException(int invalidWeight)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidStudent(weight: invalidWeight));

        Assert.Equal("Weight can't be less or equals 0", ex.Message);
    }

    [Theory]
    [InlineData("K12345678")]
    [InlineData("KKK12345678")]
    [InlineData("KB1234567")]
    [InlineData("KB123456789")]
    [InlineData("1234567890")]
    [InlineData("KB-12345678")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void StudentIdCard_InvalidFormat_ThrowsEntityValidationException(string? invalidCard)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidStudent(studentIdCard: invalidCard!));
    }

    [Theory]
    [InlineData("AA12345678")]
    [InlineData("xx87654321")]
    public void StudentIdCard_ValidFormat_SetsProperty(string validCard)
    {
        var student = CreateValidStudent(studentIdCard: validCard);
        Assert.Equal(validCard, student.StudentIdCard);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormattedString()
    {
        var student = CreateValidStudent(
            firstName: "Ivan",
            lastName: "Franko",
            passport: "AA112233",
            height: 175,
            weight: 65,
            studentIdCard: "ST11223344");

        var expected = "Student Ivan Franko. Height: 175, Weight: 65, Passport: AA112233, Student ID card: ST11223344";

        Assert.Equal(expected, student.ToString());
    }

    [Fact]
    public void RideBike()
    {
        // Arrange
        var student = CreateValidStudent();
        var logs = new List<string>();

        // Act (виконуватиметься ~6 секунд через Thread.Sleep у методі)
        student.RideBike(msg => logs.Add(msg));

        // Assert
        Assert.Equal(3, logs.Count);
        Assert.Equal("Olena Shevchenko: I get on my bicycle.", logs[0]);
        Assert.Equal("Olena Shevchenko: I ride a bicycle", logs[1]);
        Assert.Equal("Olena Shevchenko: I got off my bicycle.", logs[2]);
    }

    [Fact]
    public void LearnOnline_WhenInternetAvailable()
    {
        var student = CreateValidStudent();
        var logs = new List<string>();

        student.LearnOnline(msg => logs.Add(msg), true);

        Assert.Equal(2, logs.Count);
        Assert.Equal("Olena Shevchenko: I started studying an online course.", logs[0]);
        Assert.Equal("Olena Shevchenko: Completed an online course.", logs[1]);
    }

    [Fact]
    public void LearnOnline_WhenNoInternetConnection()
    {
        var student = CreateValidStudent();
        var logs = new List<string>();

        student.LearnOnline(msg => logs.Add(msg), false);

        Assert.Single(logs);
        Assert.Equal("Olena Shevchenko: I cannot study online because I don't have internet.", logs[0]);
    }
}
