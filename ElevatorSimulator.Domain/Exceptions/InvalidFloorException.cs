namespace ElevatorSimulator.Domain.Exceptions;

public sealed class InvalidFloorException(int selectedFloor, int minFloorAvailable, int maxFloorAvailable):
    Exception($"Floor {selectedFloor} is invalid. Select a floor from {minFloorAvailable}-{maxFloorAvailable}");