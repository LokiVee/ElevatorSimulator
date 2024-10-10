using ElevatorSimulator.Domain.Enums;

namespace ElevatorSimulator.Domain.Entities;
public abstract class Elevator : IElevator
{
    public int MaxCapacity { get; set; }
    public int CurrentFloor { get; set; } = 0;
    public double CurrentCapacity { get; set; } = 0;
    public string Name { get; set; }
    public ElevatorStatus Status { get; set; }

    public override string ToString()
    {
        return $"Elevator {Name}, Current Floor: {CurrentFloor}, Status {Status}, Capacity : {CurrentCapacity}";
    }
}
