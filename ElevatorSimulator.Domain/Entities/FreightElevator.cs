namespace ElevatorSimulator.Domain.Entities;

public sealed class FreightElevator(int elevatorId, int startingFloor = 1)
    : ElevatorBase(elevatorId, startingFloor)
{
    public override int MaxCapacity => 20;
    protected override int FloorTravelTimeMs => 800;
}