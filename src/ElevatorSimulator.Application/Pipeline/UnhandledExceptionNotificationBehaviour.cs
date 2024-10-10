using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ElevatorSimulator.Application.Pipeline;

public class UnhandledExceptionNotificationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
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
            //Log error when logging is inserted
            throw;
        }
    }
}