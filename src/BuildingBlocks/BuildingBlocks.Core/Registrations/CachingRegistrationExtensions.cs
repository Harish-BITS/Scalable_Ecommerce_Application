using System.Reflection;
using BuildingBlocks.Abstractions.Caching;

namespace BuildingBlocks.Core.Registrations;

public static class CachingRegistrationExtensions
{
    public static IServiceCollection AddCachingRequestPolicies(
        this IServiceCollection services,
        params Assembly[] assembliesToScan)
    {
        // ICacheRequest discovery and registration
        services.Scan(scan => scan
            .FromAssemblies(assembliesToScan.Any() ? assembliesToScan : AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(
                classes => classes.AssignableTo(typeof(ICacheRequest<,>)),
                false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        // IInvalidateCachePolicy discovery and registration
        services.Scan(scan => scan
            .FromAssemblies(assembliesToScan.Any() ? assembliesToScan : AppDomain.CurrentDomain.GetAssemblies())
            .AddClasses(
                classes => classes.AssignableTo(typeof(IInvalidateCachePolicy<,>)),
                false)
            .AsImplementedInterfaces()
            .WithTransientLifetime());

        return services;
    }
}
