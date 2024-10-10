using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext;
 public interface IState
{
    Task EnterState(IElevatorStateContext context);
    Task ExitState(IElevatorStateContext context);
    Task ProcessRequest(IElevatorStateContext context, Request request);
}
