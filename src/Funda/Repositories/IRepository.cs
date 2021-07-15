using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Funda.Repositories
{
    public interface IRepository<TEntity> where TEntity: class, new()
    {
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
    }
}