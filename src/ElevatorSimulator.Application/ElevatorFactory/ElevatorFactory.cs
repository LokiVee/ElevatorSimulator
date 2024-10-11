/// <summary>
/// Elevator Factory for the different types of elevators
/// </summary>
namespace ElevatorSimulator.Application.ElevatorFactory
{
    public static class ElevatorFactory
    {
        public static Elevator CreateElevator(string type, string name, int maxCapacity)
        {
            return type switch
            {
                "HighSpeed" => new HighSpeedElevator { Name = name, MaxCapacity = maxCapacity },
                "Glass" => new GlassElevator { Name = name, MaxCapacity = maxCapacity },
                _ => throw new ArgumentException("Invalid elevator type")
            };
        }
    }
}
