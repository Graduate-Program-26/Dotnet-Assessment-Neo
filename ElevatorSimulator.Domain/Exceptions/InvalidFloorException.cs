namespace ElevatorSimulator.Domain.Exceptions;

public sealed class InvalidFloorException(int selectedFloor, int minFloorAvailable, int maxFloorAvailable):
    Exception($"The floor you have selected '{selectedFloor}' is invalid. Select a floor from {minFloorAvailable}-{maxFloorAvailable}");