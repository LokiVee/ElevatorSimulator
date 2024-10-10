using ElevatorSimulator.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext.States
{
    /// <summary>
    ///    Moving State - Movement of the elevator to either the requested floor or the target floor
    /// </summary>
    public class MovingState : IState
    {
        public readonly ElevatorStatus _direction;
        private readonly bool _isPickup;
        private readonly int _delay;
        private readonly int _gotoFloor;
        public MovingState(ElevatorStatus direction, int gotoFloor, bool isPickup, int delay = 3000)
        {
            if (direction != ElevatorStatus.MovingUp && direction != ElevatorStatus.MovingDown)
                throw new ArgumentException("Invalid direction for moving state.");

            _direction = direction;
            _isPickup = isPickup;
            _delay = delay;
            _gotoFloor = gotoFloor;

        }
        public Task EnterState(IElevatorStateContext context)
        {
            context.Elevator.Status = _direction;
            return Task.CompletedTask;
        }

        public Task ExitState(IElevatorStateContext context)
        {
            return Task.CompletedTask;
        }

        public async Task ProcessRequest(IElevatorStateContext context, Request request)
        {

            do
            {
                await Task.Delay(_delay); // Simulate movement delay

                // Move the elevator in the current direction
                if (_direction == ElevatorStatus.MovingUp)
                    context.Elevator.CurrentFloor++;
                else
                    context.Elevator.CurrentFloor--;

                // Update the Status for live updates 
                context.StateHasChanged();

            } while (context.Elevator.CurrentFloor != _gotoFloor);

            // Determine if the Elevator will either be used to pick from current floor or Dropoff

            if (_isPickup)
            {
                context.TransitionToState(new PickupState());
            }
            else
            {
                context.TransitionToState(new DropOffState());
            }
            await context.ProcessRequest(request);
        }

    }
}


