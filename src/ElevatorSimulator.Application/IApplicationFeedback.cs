namespace ElevatorSimulator.Application;
public interface IApplicationFeedback
{
    /// <summary>
    /// This will fully update the status in accordance with the current status of the each elevator
    /// </summary>
    /// <param name="elevators"></param>
    /// <returns></returns>
    Task StatusUpdated(IReadOnlyCollection<IElevator> elevators);
}
