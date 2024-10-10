using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Application.Features.Requests.Commands;
using ElevatorSimulator.Application.Features.Requests.Events;
using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Enums;
using Microsoft.Extensions.Hosting;

namespace ElevatorSimulator.Application.ElevatorApplication;
public class Orchestrator : IHostedLifecycleService
{
    private readonly IApplicationFeedback _applicationFeedback;
    public List<IElevator> _elevators = new List<IElevator>();
    public Dictionary<IElevator, IElevatorStateContext> _elevatorContext = new Dictionary<IElevator, IElevatorStateContext>();

    public Orchestrator(IApplicationFeedback applicationFeedback)
    {
        _applicationFeedback = applicationFeedback;
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => ExecuteAsync(cancellationToken));
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        //TODO: Make sure threads die
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(2));
        //Load the data for elevators
        var elevator1 = new NormalElevator { Name = "1" };
        _elevators.Add(elevator1);
        var elevator1Context = new ElevatorStateContext(elevator1, StateHasChanged);
        _elevatorContext.Add(elevator1, elevator1Context);

        var elevator2 = new NormalElevator { Name = "2" };
        _elevators.Add(elevator2);
        var elevator2Context = new ElevatorStateContext(elevator2, StateHasChanged);
        _elevatorContext.Add(elevator2, elevator2Context);

        StateHasChanged();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public void StateHasChanged()
    {
        _applicationFeedback.StatusUpdated((IReadOnlyCollection<Elevator>)_elevators.AsReadOnly());
    }

    public async Task HandleRequest(ElevatorRequestCreate notification, CancellationToken cancellationToken)
    {
        var bestElevator = FindBestElevator(notification.Request);
        if (bestElevator != null)
        {
            await _elevatorContext[bestElevator].HandleRequest(notification.Request);
        }
        else
        {
            //OOPS Something is wrong need to handle this case if there is no elevator.
            //Fix up the logic on your end
            
        }
    }
    //BUG:  in finding optimal elevator
    public IElevator FindBestElevator(Request request)
    {
        // Filter elevators that can handle the request and are not busy
        var availableElevators = _elevators
            .Where(elevator => _elevatorContext[elevator].CanHandleRequest(request) &&
                              elevator.Status == ElevatorStatus.Idle) // Check if elevator is idle
            .ToList();

        if (!availableElevators.Any())
        {
            // If no elevators are available, you might want to return null or implement a waiting mechanism
            return null;
        }

        // Find the closest elevator to the requested floor
        var bestElevator = availableElevators
            .OrderBy(elevator => Math.Abs(elevator.CurrentFloor - request.CurrentFloor))
            .ThenBy(elevator => elevator.CurrentFloor) 
            .FirstOrDefault();

        return bestElevator;
    }

    private async Task SplitRequest(Request request)
    {
        var temp = new List<Request>();  //Some logic to split main request to multiple
        foreach (var req in temp)
        {
            var elevator = FindBestElevator(req);
            if (elevator == null)
            {
                await SplitRequest(request);
            }
            else
                await SubmitRequest(req, elevator);
        }
    }
    
    private async Task SubmitRequest(Request request, IElevator elevator)
    {
        var stateContext = _elevatorContext[elevator];
        await stateContext.HandleRequest(request);
    }

 
}
