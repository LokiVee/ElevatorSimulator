namespace ElevatorSimulator.Application.Features.Requests.Commands;
public class AddRequestCommand : IRequest<bool>
{
    public Request Request { get; }

    public AddRequestCommand(Request item)
    {
        Request = item;
    }
}
