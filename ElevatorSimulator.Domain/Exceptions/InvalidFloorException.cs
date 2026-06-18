namespace ElevatorSimulator.Domain.Exceptions;

/// <summary>
/// Thrown when a requested floor lies outside the valid range for the building.
/// </summary>
public sealed class InvalidFloorException(int selectedFloor, int minFloorAvailable, int maxFloorAvailable):
    Exception($"Floor {selectedFloor} is invalid. Select a floor from {minFloorAvailable}-{maxFloorAvailable}");
