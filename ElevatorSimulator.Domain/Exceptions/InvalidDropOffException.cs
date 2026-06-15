namespace ElevatorSimulator.Domain.Exceptions;

public sealed class InvalidDropOffException(int ElevatorId, int PassengerCount, int count):
    Exception($"Cannot drop off {count} passengers. There are only {PassengerCount} on board elevator {ElevatorId}.");