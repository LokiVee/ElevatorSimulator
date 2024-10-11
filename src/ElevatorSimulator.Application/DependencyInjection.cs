using ElevatorSimulator.Application.ElevatorApplication;
using ElevatorSimulator.Application.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Extensions.Logging;
using Serilog;
using System.Reflection;

namespace ElevatorSimulator.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IApplicationFeedback applicationFeedback,IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .CreateLogger();

        services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory());

        services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

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
