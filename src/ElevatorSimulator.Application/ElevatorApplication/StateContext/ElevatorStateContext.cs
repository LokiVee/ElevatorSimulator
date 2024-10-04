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
        Elevator.CurrentCapacity = _currentRequest?.ObjectWaiting ?? 0;
        _stateHasChanged.Invoke();
    }



    public bool CanHandleRequest(Request request)
    {
        //Do the check
        return request.ObjectWaiting < Elevator.MaxCapacity - _requests.Sum(i => i.ObjectWaiting);
    }

    public async Task HandleRequest(Request request)
    {
        _requests.Add(request);
        //TODO this thightly couples and must be removed
        Console.WriteLine($"{DateTime.Now} - Process request {_requests.Count} on {Elevator}");
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
            _currentRequest = _requests.First();
            _requests.RemoveAt(0);
            await ProcessRequest(_currentRequest);
        }
    }
}
