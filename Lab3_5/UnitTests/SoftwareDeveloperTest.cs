using Lab3_5.BLL.Models;
using Lab3_5.Core;

namespace Lab3_5.UnitTests;

public class SoftwareDeveloperTest
{
    private static SoftwareDeveloper CreateValidSoftwareDeveloper(
        string firstName = "Andriy",
        string lastName = "Melnyk",
        string passport = "LD942853",
        int experience = 10)
    {
        return new()
        {
            FirstName = firstName,
            LastName = lastName,
            Passport = passport,
            Experience = experience,
            PgLanguages = ["C#", "TypeScript", "JavaScript", "Python"]
        };
    }

    [Theory]
    [InlineData(-5)]
    public void Experience_LessZero_ThrowsEntityValidationException(int invalidExperience)
    {
        var ex = Assert.Throws<EntityValidationException>(() =>
            CreateValidSoftwareDeveloper(experience: invalidExperience));

        Assert.Equal("Experience can not be negative", ex.Message);
    }

    [Fact]
    public void ToString_ReturnsExpectedFormattedString()
    {
        var student = CreateValidSoftwareDeveloper();
        var expected = "Software developer Andriy Melnyk. Passport: LD942853, Experience: 10 years, Knowledge of programming languages: C#, TypeScript, JavaScript, Python";

        Assert.Equal(expected, student.ToString());
    }
}
