using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Domain.Entities;
public class NormalElevator : Elevator
{
    public NormalElevator()
    {
        MaxCapacity = 10;
    }
}
