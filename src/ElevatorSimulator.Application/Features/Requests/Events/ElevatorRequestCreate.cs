
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

public class ElevatorRequestCreateHandler : INotificationHandler<ElevatorRequestCreate>
{
    private readonly Orchestrator _orchestrator;

    public ElevatorRequestCreateHandler(Orchestrator orchestrator)
    {
        _orchestrator = orchestrator;
    }
    public async Task Handle(ElevatorRequestCreate notification, CancellationToken cancellationToken)
    {
        await _orchestrator.HandleRequest(notification, CancellationToken.None);
    }
}