using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Domain.Entities;
/// <summary>
/// The Request for each elevator request within the simulation
/// </summary>
public class Request
{
    public int CurrentFloor { get; set; }
    public int TargetFloor { get; set; }
    public int ObjectWaiting { get; set; }
}
