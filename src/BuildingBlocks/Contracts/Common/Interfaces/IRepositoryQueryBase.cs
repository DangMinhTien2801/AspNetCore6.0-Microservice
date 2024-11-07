using Contracts.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Common.Interfaces
{
    public interface IRepositoryQueryBase<T, K>
        where T : EntityBase<K>
    {
        IQueryable<T> FindAll(bool trackChanges = false);
        IQueryable<T> FindAll(bool trackChanges = false,
            params Expression<Func<T, object>>[] includeProperties);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
            bool trackChange = false);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
           bool trackChange = false, params Expression<Func<T, object>>[] includeProperties);
        Task<T?> GetByIdAsync(K id);
        Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] includeProperties);
    }
    public interface IRepositoryQueryBase<T, K, TContext>
        : IRepositoryQueryBase<T, K>
        where T : EntityBase<K>
        where TContext : DbContext
    {

    }    
}
