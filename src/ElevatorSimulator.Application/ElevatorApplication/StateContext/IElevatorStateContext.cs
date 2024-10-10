
namespace ElevatorSimulator.Application.ElevatorApplication.StateContext
{
    public interface IElevatorStateContext
    {
        IElevator Elevator { get; set; }
        public List<Request> _requests { get; set; }
        public Request _currentRequest { get; set; }
        public List<Request> _onboardRequests { get; set; }
        bool CanHandleRequest(Request request);
        Task HandleRequest(Request request);
        Task ProcessNextRequest();
        Task ProcessRequest(Request request);
        void RemoveRequest(Request request);
        void StateHasChanged();
        void TransitionToState(IState state);
    }
}