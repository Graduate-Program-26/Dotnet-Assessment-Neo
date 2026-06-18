namespace ElevatorSimulator.Domain.Entities;

/// <summary>
/// A standard passenger elevator balancing capacity and speed for everyday use.
/// </summary>
public sealed class PassengerElevator(int elevatorId, int startingFloor = 1)
    : ElevatorBase(elevatorId, startingFloor)
{
    public override int MaxCapacity => 10;
    protected override int FloorTravelTimeMs => 500;
}