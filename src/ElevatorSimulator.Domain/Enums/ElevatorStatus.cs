using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Domain.Enums;
public enum ElevatorStatus
{
    Idle,                       // The Elevator is waiting for a request
    MovingUp,                   // The Elevator is moving Up
    MovingDown,                 // The Elevator is moving Down
    PickingUp,                  // The Elevator is picking up or loading passengers
    DropOff,                    // The Elevator is dropping off or unloading passengers 

}
