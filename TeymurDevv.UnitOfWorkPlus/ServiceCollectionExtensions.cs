using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeymurDevv.UnitOfWorkPlus.Implementations;
using TeymurDevv.UnitOfWorkPlus.Interfaces;

namespace TeymurDevv.UnitOfWorkPlus;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork<TContext>(this IServiceCollection services) where TContext : DbContext
    {
        services.AddScoped<TContext>();
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork<TContext>));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<,>));

        return services;
    }
}