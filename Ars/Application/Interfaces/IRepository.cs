using Ars.Domain.Entities;

namespace Ars.Application.Interfaces;

public interface IRepository<T> where T : class, IDomainEntity
{
    IReadOnlyList<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    bool Remove(int id);
    void Initialize(List<T> sourceData);
}
