using ElevatorSimulator.Application.ElevatorApplication.StateContext.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Application.ElevatorApplication.StateContext;
internal class ElevatorStateContext 
{
    public readonly Elevator Elevator;
    private readonly Action _stateHasChanged;
    private IState _currentState;
    internal List<Request> _requests = new List<Request>();
    private Request _currentRequest;
    internal List<Request> _onboardRequests = new List<Request>(); // Requests for passengers already picked up
    public ElevatorStateContext(Elevator elevator, Action stateHasChanged)
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

        // Log the incoming request
        Console.WriteLine($"{DateTime.Now} - Processing request {_requests.Count} on {Elevator}");
        await ProcessNextRequest();
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

}
