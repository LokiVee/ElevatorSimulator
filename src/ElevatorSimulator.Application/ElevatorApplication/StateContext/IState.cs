using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext;
internal interface IState
{
    Task EnterState(ElevatorStateContext context);
    Task ExitState(ElevatorStateContext context);
    Task ProcessRequest(ElevatorStateContext context, Request request);
}
