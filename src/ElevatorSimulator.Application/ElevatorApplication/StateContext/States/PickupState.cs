using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
/// <summary>
/// The pickup state - Will be used to pick up the passengers at the requested floor
/// </summary>
public class PickupState : IState
{
    public Task EnterState(IElevatorStateContext context)
    {
        context.Elevator.Status = ElevatorStatus.PickingUp;
        return Task.CompletedTask;
    }

    public Task ExitState(IElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public async Task ProcessRequest(IElevatorStateContext context, Request request)
    {

        // Pick up passengers once we reach the requested floor
        await PickupPassengers(context, request);

        var direction = ElevatorStatus.MovingUp;
        if (request.TargetFloor < context.Elevator.CurrentFloor)
            direction = ElevatorStatus.MovingDown;
        context.TransitionToState(new MovingState(direction, request.TargetFloor, false));
        await context.ProcessRequest(request);

    }

    private async Task PickupPassengers(IElevatorStateContext context, Request request)
    {
        context._onboardRequests.Add(request); // Add the picked-up request to the onboard list
        context.Elevator.CurrentCapacity += request.ObjectWaiting;
        await Task.Delay(500); // Simulate loading time
    }
}
