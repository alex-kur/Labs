namespace Lab3_5.DAL.Entities;

public abstract class PersonEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Passport { get; set; }
}
