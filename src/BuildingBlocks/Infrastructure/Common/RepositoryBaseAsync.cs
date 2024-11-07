using Contracts.Common.Interfaces;
using Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public class RepositoryBaseAsync<T, K, TContext> : IRepositoryBaseAsync<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private readonly IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBaseAsync(TContext dbContext,
            IUnitOfWork<TContext> unitOfWork)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public IQueryable<T> FindAll(bool trackChanges = false)
        {
          return !trackChanges ? _dbContext.Set<T>().AsNoTracking() :
                _dbContext.Set<T>();
        }
        

        public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindAll(trackChanges);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, 
            bool trackChange = false)
        {
            return !trackChange ? 
                _dbContext.Set<T>().Where(expression).AsNoTracking() :
                _dbContext.Set<T>().Where(expression);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, 
            bool trackChange = false, 
            params Expression<Func<T, object>>[] includeProperties)
        {
            var items = FindByCondition(expression, trackChange);
            items = includeProperties.Aggregate(items, (current, includeProperty) => current.Include(includeProperty));
            return items;
        }
        public async Task<T?> GetByIdAsync(K id)
        {
            return await FindByCondition(x => x.Id == null ? false : x.Id.Equals(id), 
                trackChange: false).FirstOrDefaultAsync();
        }

        public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await FindByCondition(x => x.Id == null ? false : x.Id.Equals(id),
                trackChange: false, includeProperties)
                .FirstOrDefaultAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _dbContext.Database.BeginTransactionAsync();
        }
        public async Task EndTransactionAsync()
        {
            await _dbContext.SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }
        public async Task<K> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            return entity.Id;
        }

        public async Task<IList<K>> CreateListAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            return entities.Select(x => x.Id).ToList();
        }
        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await Task.Factory.StartNew(() => { });
            /*
            if (_dbContext.Entry(entity).State == EntityState.Unchanged)
                return;
            T? exist = await _dbContext.Set<T>().FindAsync(entity.Id);
            if (exist == null)
                return;
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);           
            */
        }

        public async Task UpdateListAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().UpdateRange(entities);
            await Task.Factory.StartNew(() => { });
        }
        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await Task.Factory.StartNew(() => { });
        }

        public async Task DeleteListAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await Task.Factory.StartNew(() => { });
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _unitOfWork.CommitAsync();
        }
    }
}
