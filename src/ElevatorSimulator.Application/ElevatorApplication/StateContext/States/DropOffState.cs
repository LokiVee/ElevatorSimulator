using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
/// <summary>
///  DropOff State - Will be used to drop off the passengers at the target floor
/// </summary>
public class DropOffState : IState
{
    private readonly ElevatorStatus _direction;

    public DropOffState(ElevatorStatus direction)
    {
        if (direction != ElevatorStatus.MovingUp && direction != ElevatorStatus.MovingDown)
            throw new ArgumentException("Invalid direction for moving state.");

        _direction = direction;
    }

    public async Task EnterState(IElevatorStateContext context)
    {
        context.Elevator.Status = _direction;
        await Task.Delay(400);
    }


    public Task ExitState(IElevatorStateContext context)
    {
        return Task.CompletedTask;
    }


    public async Task ProcessRequest(IElevatorStateContext context, Request request)
    {

        var dropOffRequest = context._onboardRequests
            .FirstOrDefault(r => r.TargetFloor == context.Elevator.CurrentFloor);

        if (dropOffRequest != null)
        {
            await DropOffPassengers(context, dropOffRequest);
            context._onboardRequests.Remove(dropOffRequest);
        }
        // Drop off passengers at the target floor
        await DropOffPassengers(context, request);
        context.RemoveRequest(request);
        // After drop off, transition to IdleState or another appropriate state
        context.TransitionToState(new IdleState());
        await context.ProcessRequest(request);

    }

    private async Task DropOffPassengers(IElevatorStateContext context, Request request)
    {
        context.Elevator.CurrentCapacity -= request.ObjectWaiting;
        Console.WriteLine($"Dropped off {request.ObjectWaiting} passengers at floor {request.TargetFloor}");
        await Task.Delay(500); // Simulate unloading time
    }


}
