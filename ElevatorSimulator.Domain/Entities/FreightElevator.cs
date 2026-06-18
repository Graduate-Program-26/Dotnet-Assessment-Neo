namespace ElevatorSimulator.Domain.Entities;

/// <summary>
/// A heavy-duty elevator designed for cargo, with a high passenger capacity and slower travel speed.
/// </summary>
public sealed class FreightElevator(int elevatorId, int startingFloor = 1)
    : ElevatorBase(elevatorId, startingFloor)
{
    public override int MaxCapacity => 20;
    protected override int FloorTravelTimeMs => 800;
}