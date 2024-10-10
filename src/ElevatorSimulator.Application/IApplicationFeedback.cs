namespace ElevatorSimulator.Application;
public interface IApplicationFeedback
{
    Task StatusUpdated(IReadOnlyCollection<IElevator> elevators);
}
