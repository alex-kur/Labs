using Lab3_5.Core;

namespace Lab3_5.PL;

public class AppSettings : IAppSettings
{
    public string DataFolderPath { get; set; } = "Data";
    public string FileType { get; set; } = "json";
}
