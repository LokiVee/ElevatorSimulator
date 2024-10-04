using ElevatorSimulator.Application.ElevatorApplication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ElevatorSimulator.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IApplicationFeedback applicationFeedback)
    {
        services.AddMediatR(i =>
        {
            i.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        var orchestrator = new Orchestrator(applicationFeedback);
        services.AddSingleton(orchestrator);
        services.AddSingleton<IHostedService>(orchestrator);

        return services;
    }
}
