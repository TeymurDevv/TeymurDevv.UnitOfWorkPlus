using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TeymurDevv.UnitOfWorkPlus.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> GetEntity(Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false, int skip = 0, int take = 0, params Func<IQueryable<T>, IQueryable<T>>[] includes);
    Task<List<T>> GetAll(Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false, int skip = 0, int take = 0, params Func<IQueryable<T>, IQueryable<T>>[] includes);
    Task Create(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<bool> isExists(Expression<Func<T, bool>> predicate = null);
    Task<IQueryable<T>> GetQuery(
        Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false,
        params Func<IQueryable<T>, IQueryable<T>>[] includes);
}