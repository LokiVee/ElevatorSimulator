using ElevatorSimulator.Application.ElevatorApplication.StateContext;
using ElevatorSimulator.Application.Features.Requests.Events;
using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ElevatorSimulator.Application.ElevatorApplication;
public class Orchestrator : BackgroundService
{
    private readonly IApplicationFeedback _applicationFeedback;
    public List<IElevator> _elevators = new List<IElevator>();
    public Dictionary<IElevator, IElevatorStateContext> _elevatorContext = new Dictionary<IElevator, IElevatorStateContext>();
    private readonly IConfiguration _configuration;
    public Orchestrator(IApplicationFeedback applicationFeedback,IConfiguration configuration)
    {
        _applicationFeedback = applicationFeedback;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //Load the data for elevators
        int numberOfElevators = 2;
        for (int i = 1; i <= numberOfElevators; i++)
        {
            var elevator = new NormalElevator { Name = i.ToString() };
            _elevators.Add(elevator);
            var elevatorContext = new ElevatorStateContext(elevator, StateHasChanged);
            _elevatorContext.Add(elevator, elevatorContext);
        }
        var appName = _configuration["AppSettings:NumberOfElevators"];
        StateHasChanged();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }


    /// <summary>
    /// Used to Handle max capacity of the elevator, if the max is reached the load must be distributed
    /// </summary>
    /// <param name="notification"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleRequest(ElevatorRequestCreate notification, CancellationToken cancellationToken)
    {
        var elevatorCapacity = _elevators.Max(i => i.MaxCapacity);
        if (notification.Request.ObjectWaiting > elevatorCapacity)
        {
            var objectsWaiting = notification.Request.ObjectWaiting;
            var requestTasks = new List<Task>();
            do
            {
                var requestObjects = objectsWaiting > elevatorCapacity ? elevatorCapacity : objectsWaiting;
                objectsWaiting -= requestObjects;
                var request = new Request
                {
                    CurrentFloor = notification.Request.CurrentFloor,
                    TargetFloor = notification.Request.TargetFloor,
                    ObjectWaiting = requestObjects
                };
                requestTasks.Add(AssignRequest(request));
            } while (objectsWaiting > 0);
            await Task.WhenAll(requestTasks);
        }
        else
        {
            await AssignRequest(notification.Request);
        }
    }

    private async Task AssignRequest(Request request)
    {
        var bestElevator = FindBestElevator(request);
        if (bestElevator != null)
        {
            await _elevatorContext[bestElevator].HandleRequest(request);
        }
    }


    /// <summary>
    /// Every time an update is done via the state flow this will update the status of the elevators
    /// </summary>
    public void StateHasChanged()
    {
        _applicationFeedback.StatusUpdated((IReadOnlyCollection<IElevator>)_elevators.AsReadOnly());
    }


    /// <summary>
    /// Will find the nearest elevator based on the distance and state of the elevator
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>

    public IElevator FindBestElevator(Request request)
    {
        // Filter elevators that can handle the request and are not busy
        var availableElevators = _elevators
            .Where(elevator => _elevatorContext[elevator].CanHandleRequest(request) &&
                              (elevator.Status == ElevatorStatus.Idle || elevator.Status == ElevatorStatus.MovingDown || elevator.Status == ElevatorStatus.MovingUp)) // Check if elevator is idle
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

}
