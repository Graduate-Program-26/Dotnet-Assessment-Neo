namespace ElevatorSimulator.Domain.Entities;

public sealed class HighSpeedElevator(int elevatorId, int startingFloor = 1)
    : ElevatorBase(elevatorId, startingFloor)
{
    public override int MaxCapacity => 6;
    protected override int FloorTravelTimeMs => 200;
}