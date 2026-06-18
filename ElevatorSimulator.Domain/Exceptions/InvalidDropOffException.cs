namespace ElevatorSimulator.Domain.Exceptions;

/// <summary>
/// Thrown when an attempt is made to drop off more passengers than are currently on board an elevator.
/// </summary>
public sealed class InvalidDropOffException(int ElevatorId, int PassengerCount, int count):
    Exception($"Cannot drop off {count} passengers. There are only {PassengerCount} on board elevator {ElevatorId}.");
