using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
internal class DropOffState : IState
{
    private readonly ElevatorStatus _direction;

    public DropOffState(ElevatorStatus direction)
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

            // Check if we need to drop off passengers on the current floor
            var dropOffRequest = context._onboardRequests
                .FirstOrDefault(r => r.TargetFloor == context.Elevator.CurrentFloor);

            if (dropOffRequest != null)
            {
                await DropOffPassengers(context, dropOffRequest);
                context._onboardRequests.Remove(dropOffRequest); // Remove dropped-off request
            }

        } while (context.Elevator.CurrentFloor != request.TargetFloor);

        // Now we know this request is completed, remove it from the main queue
        context._requests.Remove(request); // Remove it only after drop-off
       
        // Transition back to Idle after completing all drop-offs
        if (!context._onboardRequests.Any())
        {
            context.TransitionToState(new IdleState());
            await context.ProcessNextRequest();
        }
    }

    private async Task DropOffPassengers(ElevatorStateContext context, Request request)
    {
        context.Elevator.CurrentCapacity -= request.ObjectWaiting;
        Console.WriteLine($"Dropped off {request.ObjectWaiting} passengers at floor {request.TargetFloor}");
        await Task.Delay(500); // Simulate unloading time
    }
}
