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
            await Task.Delay(3000);
            if (_direction == ElevatorStatus.MovingUp) 
                context.Elevator.CurrentFloor++; 
            else 
                context.Elevator.CurrentFloor--;

            context.StateHasChanged();
        }while(context.Elevator.CurrentFloor != request.CurrentFloor);
        context.TransitionToState(new LoadingState());
        await context.ProcessRequest(request);
    }
}
