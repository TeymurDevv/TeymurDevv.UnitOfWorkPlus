using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeymurDevv.UnitOfWorkPlus.Implementations;
using TeymurDevv.UnitOfWorkPlus.Interfaces;

namespace TeymurDevv.UnitOfWorkPlus.Implementations
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
    {
        private readonly TContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Hashtable _repositories = new();

        public UnitOfWork(TContext context, IServiceProvider serviceProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            RegisterRepositories();
        }

        private void RegisterRepositories()
        {
            var repoTypes = _serviceProvider.GetServices(typeof(object))
                .Where(s => s?.GetType().GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)) ?? false);

            foreach (var repo in repoTypes)
            {
                var entityType = repo.GetType().GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>))
                    .GetGenericArguments()[0];

                var property = GetType().GetProperty(entityType.Name + "Repository",
                    BindingFlags.Public | BindingFlags.Instance);

                if (property != null)
                {
                    property.SetValue(this, repo);
                }

                _repositories[entityType] = repo;
            }
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            if (!_repositories.ContainsKey(typeof(T)))
            {
                var repositoryType = typeof(Repository<,>).MakeGenericType(typeof(T), typeof(TContext));
                var repositoryInstance = Activator.CreateInstance(repositoryType, _context);
                _repositories[typeof(T)] = repositoryInstance;
            }

            return (IRepository<T>)_repositories[typeof(T)];
        }

        // Auto-generate properties for repositories dynamically
        public IRepository<T> Repository<T>() where T : class => GetRepository<T>();
    }
}