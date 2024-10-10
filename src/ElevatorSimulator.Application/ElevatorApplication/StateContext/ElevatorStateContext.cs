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
    public List<Request> _requests { get; set; }
    public Request _currentRequest { get; set; }
    public List<Request> _onboardRequests { get; set; } // Requests for passengers already picked up
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
        Elevator.CurrentCapacity = _onboardRequests.Sum(r => r.ObjectWaiting); // Update based on total onboard passengers
        _stateHasChanged.Invoke();
    }

    public bool CanHandleRequest(Request request)
    {
        ////Do the check
        //return request.ObjectWaiting < Elevator.MaxCapacity - _requests.Sum(i => i.ObjectWaiting);
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
    //BUG: Not checking with loading and unloading for more requests on the same floor.  This must be fixed in the state flow
    public async Task ProcessRequest(Request request)
    {
        await _currentState.ProcessRequest(this, request);
    }
    public async Task ProcessNextRequest()
    {
        if (_currentState is IdleState && _requests.Any())
        {
            // Sort requests by the most efficient route (e.g., in the current direction of travel)
            _currentRequest = FindNextOptimalRequest();
            await ProcessRequest(_currentRequest);
        }
    }
    private Request FindNextOptimalRequest()
    {
        // Return the closest request by current floor, prioritizing direction alignment (MovingUp or MovingDown)
        return _requests.OrderBy(r => Math.Abs(r.CurrentFloor - Elevator.CurrentFloor)).First();
    }

    public void RemoveRequest(Request request)
    {
        _requests.Remove(request);
        if (!_requests.Any() && !_onboardRequests.Any())
        {
            TransitionToState(new IdleState());
        }
    }

}
