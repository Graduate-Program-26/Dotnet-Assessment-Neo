namespace ElevatorSimulator.Domain.Entities;

public sealed class PassengerElevator(int elevatorId, int startingFloor = 1)
    : ElevatorBase(elevatorId, startingFloor)
{
    public override int MaxCapacity => 10;
    protected override int FloorTravelTimeMs => 500;
}