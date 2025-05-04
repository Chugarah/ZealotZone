using Core.Factories.Data;
using Core.Factories.User;
using Core.Interfaces.Data;
using Core.Interfaces.DTos;
using Core.Interfaces.Factories;
using Core.Interfaces.User;
using Core.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

/// <summary>
/// This is a Composition Root pattern
/// https://medium.com/@cfryerdev/dependency-injection-composition-root-418a1bb19130
/// API -> Core
/// Infrastructure -> Core
/// API -> Infrastructure (only for DI registration)
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<IRepositoryResultFactory, RepositoryResultFactory>();
        services.AddScoped<IUserService, UserService>();

        // Register factories
        services.AddScoped<IUserDtoFactory, UserDtoFactory>();

        // Returning the services so it could be used
        // in our API layer where we are registering the services
        // using a Composition Root pattern
        return services;
    }
}