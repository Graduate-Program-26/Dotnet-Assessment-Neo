namespace ElevatorSimulator.Domain.Entities;

/// <summary>
/// A high-speed elevator optimised for rapid transit, with reduced capacity and a fast floor travel time.
/// </summary>
public sealed class HighSpeedElevator(int elevatorId, int startingFloor = 1)
    : ElevatorBase(elevatorId, startingFloor)
{
    public override int MaxCapacity => 6;
    protected override int FloorTravelTimeMs => 200;
}