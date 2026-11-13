using Lab3_5.Core;
using System.Text.RegularExpressions;

namespace Lab3_5.BLL.Models;

public abstract class Person
{
    public required string FirstName
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityValidationException("First name can't be empty");

            field = value;
        }
    }

    public required string LastName
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityValidationException("Last name can't be empty");

            field = value;
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// AA000000
    /// </summary>
    public required string Passport
    {
        get => field;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new EntityValidationException("Passport can't be empty");

            if (!Regex.IsMatch(value, "^[A-Za-z]{2}\\d{6}$"))
                throw new EntityValidationException("Passport must be in the format AA000000");

            field = value;
        }
    }

    public void RideBike(Action<string> log)
    {
        log($"{FullName}: I get on my bicycle.");
        Thread.Sleep(1000);
        log($"{FullName}: I ride a bicycle");
        Thread.Sleep(5000);
        log($"{FullName}: I got off my bicycle.");
    }

    public void LearnOnline(Action<string> log, bool haveInternetConnection)
    {
        if (!haveInternetConnection)
        {
            log($"{FullName}: I cannot study online because I don't have internet.");
            return;
        }

        log($"{FullName}: I started studying an online course.");
        Thread.Sleep(2000);
        log($"{FullName}: Completed an online course.");
    }

    public override string ToString()
    {
        return ToStringImp();
    }

    protected abstract string ToStringImp();
}
