using Ars.Application.Interfaces;
using Ars.Domain.Entities;

namespace Ars.Data;

public class InMemoryRepository<T> : IRepository<T> where T : class, IDomainEntity
{
    private readonly List<T> _items = new();

    public void Add(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _items.Add(entity);
        entity.Id = _items.Max(item => item.Id) + 1;
    }

    public T? GetById(int id) => _items.FirstOrDefault(item => item.Id == id);

    public IReadOnlyList<T> GetAll() => _items.AsReadOnly();

    public bool Remove(int id)
    {
        var entity = GetById(id);
        return entity != null && _items.Remove(entity);
    }

    public void Initialize(List<T> sourceData)
    {
        _items.Clear();
        _items.AddRange(sourceData);
    }
}
