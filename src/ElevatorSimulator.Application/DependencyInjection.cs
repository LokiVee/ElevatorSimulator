using ElevatorSimulator.Application.ElevatorApplication;
using ElevatorSimulator.Application.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ElevatorSimulator.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IApplicationFeedback applicationFeedback,IConfiguration configuration)
    {
        services.AddMediatR(i =>
        {
            i.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            i.AddOpenBehavior(typeof(UnhandledExceptionNotificationBehaviour<,>));
        });
        var orchestrator = new Orchestrator(applicationFeedback, configuration);
        services.AddSingleton(orchestrator);
        services.AddSingleton<IHostedService>(orchestrator);
        return services;
    }
}
