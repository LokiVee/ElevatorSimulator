using ElevatorSimulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States
{
    /// <summary>
    ///    Moving State - Used to move the elevator to the requested floor
    /// </summary>
    public class MovingState : IState
    {
        public readonly ElevatorStatus _direction;
        private readonly bool _isPickup;
        private readonly int _targetFloor;
        public MovingState(ElevatorStatus direction, int targetFloor, bool isPickup)
        {
            if (direction != ElevatorStatus.MovingUp && direction != ElevatorStatus.MovingDown)
                throw new ArgumentException("Invalid direction for moving state.");

            _direction = direction;
            _isPickup = isPickup;
            _targetFloor = targetFloor;

        }
        public async Task EnterState(IElevatorStateContext context)
        {
            context.Elevator.Status = _direction;
            await Task.Delay(400);
        }

        public Task ExitState(IElevatorStateContext context)
        {
            return Task.CompletedTask;
        }

        public async Task ProcessRequest(IElevatorStateContext context, Request request)
        {
            do
            {
                await Task.Delay(3000); // Simulate movement delay

                // Move the elevator in the current direction
                if (_direction == ElevatorStatus.MovingUp)
                    context.Elevator.CurrentFloor++;
                else
                    context.Elevator.CurrentFloor--;

                // Update the Status for live updates 
                context.StateHasChanged();

            } while (context.Elevator.CurrentFloor != request.CurrentFloor);

            // Determine if the Elevator will either be used to pick from current floor or Dropoff

            if (_isPickup)
            {
                context.TransitionToState(new PickupState(_direction,_targetFloor));
            }
            else
            {
                context.TransitionToState(new DropOffState(_direction));
            }
            await context.ProcessRequest(request);
        }

   }    
}


