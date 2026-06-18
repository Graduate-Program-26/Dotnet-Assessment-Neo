namespace ElevatorSimulator.Domain.Interfaces;

using ElevatorSimulator.Domain.Enums;

/// <summary>
/// Defines the contract for an elevator unit within the simulation.
/// </summary>
public interface IElevator
{
    int ElevatorId { get; }
    int CurrentFloor { get; }
    int PassengerCount { get; }
    int MaxCapacity { get; }
    bool IsAtMaxCapacity { get; }
    ElevatorDirection Direction { get; }
    ElevatorState State { get; }

    void PickUpPassengers(int count);
    void DropOffPassengers(int count);

    Task MoveToFloor(int floor);
}
