using Lab3_5.Core;
using Lab3_5.DAL.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Lab3_5.DAL;

public class EntityContext<T>(IAppSettings appSettings, IServiceProvider serviceProvider) : IEntityContext<T> where T : PersonEntity
{
    private string FileName => $"{typeof(T).Name[..^1]}ies.{DataProvider.DataFormat}";
    private string FilePath => Path.Combine(appSettings.DataFolderPath, FileName);

    private IDataProvider<T> DataProvider => serviceProvider.GetRequiredKeyedService<IDataProvider<T>>(appSettings.FileType);

    public List<T> Load()
    {
        if (!File.Exists(FilePath))
            return [];

        var buffer = File.ReadAllBytes(FilePath);
        return DataProvider.Deserialize(buffer);
    }

    public void Save(List<T> entities)
    {
        var buffer = DataProvider.Serialize(entities);
        File.WriteAllBytes(FilePath, buffer);
    }
}
