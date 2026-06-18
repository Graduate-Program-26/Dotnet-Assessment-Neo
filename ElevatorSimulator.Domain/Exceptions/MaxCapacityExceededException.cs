namespace ElevatorSimulator.Domain.Exceptions;

/// <summary>
/// Thrown when boarding additional passengers would exceed an elevator's maximum capacity.
/// </summary>
public sealed class MaxCapacityExeceededException(int elevatorId, int maxCapacity, int amountOfPassengers) :
    Exception($"Elevator {elevatorId} cannot carry {amountOfPassengers} passengers. The max capacity is {maxCapacity}.");
