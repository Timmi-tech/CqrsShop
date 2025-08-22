using Application.Interfaces.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class ReadRepository<T>(RepositoryWriteDbContext repositoryReadContextFactory) : IReadRepository<T> where T : class
    {
        protected readonly RepositoryWriteDbContext _repositoryReadContextFactory = repositoryReadContextFactory;

        public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
              _repositoryReadContextFactory.Set<T>()
                .AsNoTracking() :
            _repositoryReadContextFactory.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
            !trackChanges ?
                _repositoryReadContextFactory.Set<T>()
                    .Where(expression)
                    .AsNoTracking() :
                _repositoryReadContextFactory.Set<T>()
                    .Where(expression);
        public void Create(T entity) => _repositoryReadContextFactory.Set<T>().Add(entity);
        public void Update(T entity) => _repositoryReadContextFactory.Set<T>().Update(entity);
        public void Delete(T entity) => _repositoryReadContextFactory.Set<T>().Remove(entity);
    }
    }
