using Lab3_5.BLL.Models;

namespace Lab3_5.BLL.EntityServices
{
    public interface IEntityService<TModel> where TModel : Person
    {
        void Add(TModel model);
        IReadOnlyList<TModel> GetAll();
        TModel GetAt(int index);
        void RemoveAt(int index);
    }
}