
using ElevatorSimulator.Application.ElevatorApplication;

namespace ElevatorSimulator.Application.Features.Requests.Events;
public class ElevatorRequestCreate : INotification
{
    public ElevatorRequestCreate(Request request)
    {
        Request = request;
    }

    public Request Request { get; }
}

internal class ElevatorRequestCreateHandler : INotificationHandler<ElevatorRequestCreate>
{
    private readonly Orchestrator _orchestrator;

    public ElevatorRequestCreateHandler(Orchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }
    public Task Handle(ElevatorRequestCreate notification, CancellationToken cancellationToken)
    {
        return _orchestrator.HandleRequest(notification, cancellationToken);
    }
}