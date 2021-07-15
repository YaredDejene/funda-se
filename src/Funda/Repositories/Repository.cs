using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Funda.Data;

namespace Funda.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly ApplicationDbContext _dbContext;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IQueryable<TEntity> GetAll()
        {
            try 
            { 
                return _dbContext.Set<TEntity>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrive entities: {ex.Message}");
            }
        }

        public IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> filter)
        {
            if(filter == null)
            {
                throw new ArgumentNullException($"{nameof(Filter)} filter must not be null.");
            }

            try 
            { 
                return _dbContext.Set<TEntity>().Where(filter);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unable to retrive filtered entities: {ex.Message}");
            }
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null.");
            }

            try 
            { 
                await _dbContext.AddAsync(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"${nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if(entity == null)
            {
                throw new ArgumentNullException($"{nameof(AddAsync)} entity must not be null.");
            }

            try 
            { 
                _dbContext.Update(entity);
                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception($"${nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}