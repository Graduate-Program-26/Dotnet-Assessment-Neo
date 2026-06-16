namespace ElevatorSimulator.Application.Services;

using ElevatorSimulator.Domain.Exceptions;

public sealed class FloorManager(int minFloor, int maxFloor)
{
    public int MinFloor { get; } = minFloor;
    public int MaxFloor { get; } = maxFloor;

    public void ValidateFloor(int floor)
    {
        if (floor < MinFloor || floor > MaxFloor)
            throw new InvalidFloorException(floor, MinFloor, MaxFloor);
    }
}