namespace ElevatorSimulator.Domain.Interfaces;

using ElevatorSimulator.Domain.Enums;

public interface IElevator
{
    int ElevatorId { get; }
    int CurrentFloor { get; }
    int PassengerCount { get; }
    int MaxCapacity { get; }
    bool IsAtMaxCapacity { get; }
    ElevatorDirection Direction { get; }
    ElevatorState State { get; }

    void pickUpPassengers(int count);
    void dropOffPassengers(int count);

    Task MoveToFloor(int floor);
}