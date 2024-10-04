using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
internal class IdleState : IState
{
    public async Task EnterState(ElevatorStateContext context)
    {
        context.Elevator.Status = ElevatorStatus.Idle;
        await Task.Delay(400);
    }

    public Task ExitState(ElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public Task ProcessRequest(ElevatorStateContext context, Request request)
    {
        var direction = ElevatorStatus.MovingUp;
        if (request.CurrentFloor < context.Elevator.CurrentFloor)
            direction = ElevatorStatus.MovingDown;
        context.TransitionToState(new PickupState(direction));
        return context.ProcessRequest(request);
    }
}