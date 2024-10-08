using ElevatorSimulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States
{
    //public class MovingState : IState
    //{
    //    private readonly ElevatorStatus _direction;
    //    public MovingState(ElevatorStatus direction) 
    //    {
    //        if (direction != ElevatorStatus.MovingUp && direction != ElevatorStatus.MovingDown)
    //            throw new ArgumentException("Invalid direction for moving state.");

    //        _direction = direction;

    //    }
    //    public async Task EnterState(ElevatorStateContext context)
    //    {
    //        context.Elevator.Status = _direction;
    //        await Task.Delay(400);
    //    }

    //    public Task ExitState(ElevatorStateContext context)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    public async Task ProcessRequest(ElevatorStateContext context, Request request)
    //    {
    //        do
    //        {
    //            await Task.Delay(3000); // Simulate movement delay

    //            // Move the elevator in the current direction
    //            if (_direction == ElevatorStatus.MovingUp)
    //                context.Elevator.CurrentFloor++;
    //            else
    //                context.Elevator.CurrentFloor--;

    //            // Update the Status for live updates 
    //            context.StateHasChanged();

    //        } while (context.Elevator.CurrentFloor != request.CurrentFloor);

    //        // Pick up passengers once we reach the requested floor
    //        await PickupPassengers(context, request);

    //        // After pickup, proceed with onboard requests and drop-offs
    //        var direction = request.TargetFloor > context.Elevator.CurrentFloor
    //            ? ElevatorStatus.MovingUp
    //            : ElevatorStatus.MovingDown;

    //        context.TransitionToState(new DropOffState(direction));
    //        await context.ProcessRequest(request);
    //    }

    //    private async Task PickupPassengers(ElevatorStateContext context, Request request)
    //    {
    //        context._onboardRequests.Add(request); // Add the picked-up request to the onboard list
    //        context.Elevator.CurrentCapacity += request.ObjectWaiting;
    //        await Task.Delay(500); // Simulate loading time
    //    }
    //}
}


