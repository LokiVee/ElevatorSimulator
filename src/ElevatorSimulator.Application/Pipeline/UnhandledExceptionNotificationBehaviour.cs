using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ElevatorSimulator.Application.Pipeline;

public class UnhandledExceptionNotificationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<UnhandledExceptionNotificationBehaviour<TRequest, TResponse>> _logger;
    public UnhandledExceptionNotificationBehaviour()
    {
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
#if DEBUG
            Debugger.Break();
#endif

            _logger.LogError(ex, $"Unhandled exception occurred while processing request {request}", typeof(TRequest).Name, request);
            throw;
        }
    }
}