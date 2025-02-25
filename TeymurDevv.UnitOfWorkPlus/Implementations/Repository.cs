using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeymurDevv.UnitOfWorkPlus.Interfaces;

namespace TeymurDevv.UnitOfWorkPlus.Implementations
{
    public class Repository<T, TContext> : IRepository<T>
        where T : class
        where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly DbSet<T> _table;

        public Repository(TContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _table = _context.Set<T>();
        }

         public async Task Create(T entity)
        {
            try
            {
                var result = _context.Entry(entity);
                result.State = EntityState.Added;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Delete(T entity)
        {
            try
            {
                var result = _context.Entry(entity);
                result.State = EntityState.Deleted;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>> predicate = null, bool AsnoTracking=false, int skip = 0, int take = 0, params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            try
            {
                IQueryable<T> query = _table;

                // Check if includes array is not null and has elements
                if (includes != null && includes.Length > 0)
                {
                    foreach (var include in includes)
                    {
                        query = include(query);
                    }
                }

                // Apply the predicate if provided
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                // Apply skip and take logic
                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }

                // Execute the query and return the list
                if(AsnoTracking is true){
                    query = query.AsNoTracking();
                };
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the full exception details for better debugging
                throw new Exception("Error in GetAll: " + ex.Message, ex);
            }
        }

        public async Task<T> GetEntity(Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false, int skip = 0, int take = 0, params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            try
            {
                IQueryable<T> query = _table;

                // Check if includes array is not null and has elements
                if (includes != null && includes.Length > 0)
                {
                    foreach (var include in includes)
                    {
                        query = include(query);
                    }
                }

                // Apply the predicate first if provided
                if (predicate != null)
                {
                    query = query.Where(predicate);
                }

                if (skip > 0)
                {
                    query = query.Skip(skip);
                }

                if (take > 0)
                {
                    query = query.Take(take);
                }
                if (AsnoTracking is true)
                {
                    query = query.AsNoTracking();
                };
                return await query.FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetEntity: " + ex.Message, ex);
            }
        }

        public Task<IQueryable<T>> GetQuery(
      Expression<Func<T, bool>> predicate = null, bool AsnoTracking = false,
      params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            try
            {
                IQueryable<T> query = _table.AsQueryable();

                if (includes != null && includes.Length > 0)
                {
                    foreach (var include in includes)
                    {
                        query = include(query);
                    }
                }

                if (predicate != null)
                {
                    query = query.Where(predicate);
                }
                if (AsnoTracking is true)
                {
                    query = query.AsNoTracking();
                };
                return Task.FromResult(query);
            }
            catch (Exception ex)
            {
                // Provide more detailed error information
                throw new Exception($"Error in GetQuery for type {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<bool> isExists(Expression<Func<T, bool>> predicate = null)
        {
            try
            {
                return predicate is null ? false : await _table.AnyAsync(predicate);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task Update(T entity)
        {
            try
            {
                var result = _context.Entry(entity);
                result.State = EntityState.Modified;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}