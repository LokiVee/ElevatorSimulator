using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Domain.Entities
{
    public interface IElevator
    {
        double CurrentCapacity { get; set; }
        int CurrentFloor { get; set; }
        int MaxCapacity { get; set; }
        string Name { get; set; }
        ElevatorStatus Status { get; set; }

    }
}