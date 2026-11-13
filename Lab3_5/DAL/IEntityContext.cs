using Lab3_5.DAL.Entities;

namespace Lab3_5.DAL
{
    public interface IEntityContext<T> where T : PersonEntity
    {
        List<T> Load();
        void Save(List<T> entities);
    }
}