using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
/// <summary>
/// IdleState - Used to indicate the elevator is ready to be in motion.
/// </summary>
public class IdleState : IState
{
    public async Task EnterState(IElevatorStateContext context)
    {
        context.Elevator.Status = ElevatorStatus.Idle;
        await Task.Delay(400);
        await context.ProcessNextRequest();
    }

    public Task ExitState(IElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public Task ProcessRequest(IElevatorStateContext context, Request request)
    {
        var direction = ElevatorStatus.MovingUp;
        if (request.CurrentFloor < context.Elevator.CurrentFloor) 
            direction = ElevatorStatus.MovingDown;
        context.TransitionToState(new MovingState(direction, request.CurrentFloor, true));
        return context.ProcessRequest(request);
    }
}