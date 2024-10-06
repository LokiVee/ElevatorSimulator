using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
internal class PickupState : IState
{
    private readonly ElevatorStatus _direction;

    public PickupState(ElevatorStatus direction)
    {
        if (direction != ElevatorStatus.MovingUp && direction != ElevatorStatus.MovingDown)
            throw new ArgumentException("Invalid direction for moving state.");

        _direction = direction;
    }

    public async Task EnterState(ElevatorStateContext context)
    {
        context.Elevator.Status = _direction;
        await Task.Delay(400);
    }

    public Task ExitState(ElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public async Task ProcessRequest(ElevatorStateContext context, Request request)
    {
        do
        {
            await Task.Delay(3000); // Simulate movement delay

            // Move the elevator in the current direction
            if (_direction == ElevatorStatus.MovingUp)
                context.Elevator.CurrentFloor++;
            else
                context.Elevator.CurrentFloor--;

            context.StateHasChanged();

            // Check if there's a request to pick up on this floor, in the same direction
            var intermediateRequest = context._requests
                .FirstOrDefault(r => r.CurrentFloor == context.Elevator.CurrentFloor &&
                                     CanPickupIntermediateRequest(context, r));

            if (intermediateRequest != null && context.CanHandleRequest(intermediateRequest))
            {
                // Stop to pick up additional passengers and handle the request
                context._requests.Remove(intermediateRequest);
                await PickupPassengers(context, intermediateRequest);
            }

        } while (context.Elevator.CurrentFloor != request.CurrentFloor);

        // Pick up passengers once we reach the requested floor
        await PickupPassengers(context, request);

        // After pickup, proceed with onboard requests and drop-offs
        var direction = request.TargetFloor > context.Elevator.CurrentFloor
            ? ElevatorStatus.MovingUp
            : ElevatorStatus.MovingDown;

        context.TransitionToState(new DropOffState(direction));
        await context.ProcessRequest(request);
    }



    private bool CanPickupIntermediateRequest(ElevatorStateContext context, Request request)
    {
        // Only pick up requests that are moving in the same direction
        if (_direction == ElevatorStatus.MovingUp && request.TargetFloor > request.CurrentFloor)
            return true;

        if (_direction == ElevatorStatus.MovingDown && request.TargetFloor < request.CurrentFloor)
            return true;

        return false;
    }

    private async Task PickupPassengers(ElevatorStateContext context, Request request)
    {
        context._onboardRequests.Add(request); // Add the picked-up request to the onboard list
        context.Elevator.CurrentCapacity += request.ObjectWaiting;
        Console.WriteLine($"Picked up {request.ObjectWaiting} passengers at floor {request.CurrentFloor}");
        await Task.Delay(500); // Simulate loading time
    }
}
