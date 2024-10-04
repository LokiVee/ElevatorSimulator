using Microsoft.Extensions.DependencyInjection;

namespace ElevatorSimulator.Domain;
public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        //Can add if there is any, normally should not have
        return services;
    }
}
