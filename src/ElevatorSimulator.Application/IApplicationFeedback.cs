namespace ElevatorSimulator.Application;
public interface IApplicationFeedback
{
    Task StatusUpdated(IReadOnlyCollection<Elevator> elevators);
}
