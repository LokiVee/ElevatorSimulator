using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
/// <summary>
///  DropOff State - Will be used to drop off the passengers at the target floor
/// </summary>
public class DropOffState : IState
{
    public Task EnterState(IElevatorStateContext context)
    {
        context.Elevator.Status = ElevatorStatus.DropOff;
        return Task.CompletedTask;
    }

    public Task ExitState(IElevatorStateContext context)
    {
        return Task.CompletedTask;
    }

    public async Task ProcessRequest(IElevatorStateContext context, Request request)
    {

        context.Elevator.CurrentCapacity -= request.ObjectWaiting;
        await Task.Delay(500);// Delay to Dropoff Passengers 

        // Drop off passengers at the target floor
        context.RemoveRequest(request);
        // After drop off, transition to IdleState or another appropriate state
        context.TransitionToState(new IdleState());

    }


}
