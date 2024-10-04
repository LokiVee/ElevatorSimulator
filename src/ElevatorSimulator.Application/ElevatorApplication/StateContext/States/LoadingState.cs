using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
internal class LoadingState : IState
{
    public Task EnterState(ElevatorStateContext context)
    {
        context.Elevator.Status = Domain.Enums.ElevatorStatus.Idle;
        return Task.CompletedTask;
    }

    public Task ExitState(ElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public async Task ProcessRequest(ElevatorStateContext context, Request request)
    {
        var counter = 0;
        do
        {
            await Task.Delay(200);
            counter++;
        } while (request.ObjectWaiting == counter);

        if(request.TargetFloor == context.Elevator.CurrentFloor)
        {
            context.TransitionToState(new IdleState());
            await context.ProcessNextRequest();
        }
        else
        {
            var direction = ElevatorStatus.MovingUp;
            if (request.TargetFloor < context.Elevator.CurrentFloor)
                direction = ElevatorStatus.MovingDown;
            context.TransitionToState(new DropOffState(direction));
            await context.ProcessRequest(request);
        }
    }
}
