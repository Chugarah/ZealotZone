
using Core.Interfaces.Data;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// This is a Composition Root pattern
/// https://medium.com/@cfryerdev/dependency-injection-composition-root-418a1bb19130
/// API -> Core
/// Infrastructure -> Core
/// API -> Infrastructure (only for DI registration)
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the required services and configurations for the infrastructure layer.
    /// This includes setting up the database context and dependency injection for
    /// infrastructure-specific functionalities such as the UnitOfWork.
    /// </summary>
    /// <param name="services">The IServiceCollection to which the dependencies will be added.</param>
    /// <param name="connectionString">The connection string used to configure the database context.</param>
    /// <returns>The updated IServiceCollection with the infrastructure dependencies added.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString
    )
    {
        // We are taking the connection string from the API layer.
        services.AddDbContext<DataContext>(options =>
            options.UseAzureSql(
                connectionString,
                b => b.MigrationsAssembly(typeof(DataContext).Assembly.FullName)
            )
        );
        // Got help from Phind AI to add this line regarding the UnitOfWork
        services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(
            provider.GetRequiredService<DataContext>()
        ));
        return services;
    }
}