namespace ElevatorSimulator.Domain.Exceptions;

public sealed class MaxCapacityExeceededException(int elevatorId, int maxCapacity, int amountOfPassengers) : 
    Exception($"Elevator {elevatorId} cannot carry {amountOfPassengers} passengers. The max capacity is {maxCapacity}.");