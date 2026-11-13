namespace Lab3_5.Core;

public interface IDataProvider<T> where T : class
{
    string DataFormat { get; }
    byte[] Serialize(List<T> data);
    List<T> Deserialize(byte[] buffer);
}
