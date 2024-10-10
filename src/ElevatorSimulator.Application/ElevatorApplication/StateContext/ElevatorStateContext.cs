using ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext;
public class ElevatorStateContext : IElevatorStateContext
{
    public IElevator Elevator { get; set; }

    public readonly Action _stateHasChanged;

    public IState _currentState;
    public List<Request> _requests { get; set; } = new List<Request>();
    public Request _currentRequest { get; set; }
    public List<Request> _onboardRequests { get; set; } = new List<Request>(); // Requests for passengers already picked up
    public ElevatorStateContext(IElevator elevator, Action stateHasChanged)
    {
        Elevator = elevator;
        _stateHasChanged = stateHasChanged;
        _currentState = new IdleState();
    }

    public void TransitionToState(IState state)
    {
        state.ExitState(this);
        _currentState = state;
        _currentState.EnterState(this);
        StateHasChanged();

        if (_currentState is IdleState && _requests.Any())
        {
            ProcessNextRequest().Wait();
        }
    }

    public void StateHasChanged()
    {
        _stateHasChanged.Invoke();
    }

    public bool CanHandleRequest(Request request)
    {
        var capacityCheck = request.ObjectWaiting <= Elevator.MaxCapacity - _requests.Sum(i => i.ObjectWaiting);
        return capacityCheck;

    }
    public async Task HandleRequest(Request request)
    {
        _requests.Add(request);
        if (_currentState is IdleState)
        {
            await ProcessNextRequest();
        }
    }

    public async Task ProcessRequest(Request request)
    {
        await _currentState.ProcessRequest(this, request);
    }
    /// <summary>
    ///  Processing of the multiple requests 
    /// </summary>
    /// <returns></returns>
    public async Task ProcessNextRequest()
    {
        if (_currentState is IdleState && _requests.Any())
        {
            // Sort requests by the most efficient route (e.g., in the current direction of travel)
            _currentRequest = FindNextOptimalRequest();
            await ProcessRequest(_currentRequest);
        }
    }

    /// <summary>
    ///Return the closest request by current floor, prioritizing direction alignment (MovingUp or MovingDown)
    /// </summary>
    /// <returns></returns>
    private Request FindNextOptimalRequest()
    {
   
        return _requests.OrderBy(r => Math.Abs(r.CurrentFloor - Elevator.CurrentFloor)).First();
    }

    /// <summary>
    /// This method will remove the request from the On board requests and the original request list
    /// </summary>
    /// <param name="request"></param>
    public void RemoveRequest(Request request)
    {
        _requests.Remove(request);
        _onboardRequests.Remove(request);
    }
}
