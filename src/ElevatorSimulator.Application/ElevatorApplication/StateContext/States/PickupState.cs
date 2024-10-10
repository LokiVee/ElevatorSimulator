using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
public class PickupState : IState
{
    private readonly ElevatorStatus _direction;
    private readonly int _targetFloor;
    public PickupState(ElevatorStatus direction, int targetFloor)
    {
        if (direction != ElevatorStatus.MovingUp && direction != ElevatorStatus.MovingDown)
            throw new ArgumentException("Invalid direction for moving state.");

        _direction = direction;
    }

    public async Task EnterState(IElevatorStateContext context)
    {
        context.Elevator.Status = ElevatorStatus.PickingUp;
        await Task.Delay(3000);
    }

    public Task ExitState(IElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public async Task ProcessRequest(IElevatorStateContext context, Request request)
    {
        
            // Pick up passengers once we reach the requested floor
            await PickupPassengers(context, request);

            // After pickup, transition back to MovingState to continue to the target floor
            context.TransitionToState(new MovingState(_direction, _targetFloor, false));
        await context.ProcessRequest(request);

    }



    private bool CanPickupIntermediateRequest(IElevatorStateContext context, Request request)
    {
        // Only pick up requests that are moving in the same direction
        if (_direction == ElevatorStatus.MovingUp && request.TargetFloor > request.CurrentFloor)
            return true;

        if (_direction == ElevatorStatus.MovingDown && request.TargetFloor < request.CurrentFloor)
            return true;

        return false;
    }

    private async Task PickupPassengers(IElevatorStateContext context, Request request)
    {
        context._onboardRequests.Add(request); // Add the picked-up request to the onboard list
        context.Elevator.CurrentCapacity += request.ObjectWaiting;
        Console.WriteLine($"Picked up {request.ObjectWaiting} passengers at floor {request.CurrentFloor}");
        await Task.Delay(500); // Simulate loading time
    }
}
