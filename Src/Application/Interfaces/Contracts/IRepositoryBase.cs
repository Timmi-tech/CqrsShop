using System.Linq.Expressions;

namespace Application.Interfaces.Contracts
{
    public interface IReadRepository<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);
}

public interface IWriteRepository<T>
{
    void Create(T entity); 
    void Update(T entity); 
    void Delete(T entity); 
}

}