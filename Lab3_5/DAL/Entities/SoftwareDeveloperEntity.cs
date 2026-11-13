namespace Lab3_5.DAL.Entities;

public class SoftwareDeveloperEntity : PersonEntity
{
    public required int Experience { get; set; }
    public required List<string> PgLanguages { get; set; }
}
