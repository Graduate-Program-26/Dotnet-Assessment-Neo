namespace ElevatorSimulator.Domain.Entities;

using ElevatorSimulator.Domain.Enums;
using ElevatorSimulator.Domain.Exceptions;
using ElevatorSimulator.Domain.Interfaces;

public abstract class ElevatorBase : IElevator
{
    public int ElevatorId { get; }
    public int CurrentFloor { get; protected set; }
    public int PassengerCount { get; protected set; }
    public ElevatorDirection Direction { get; protected set; } = ElevatorDirection.Stationary;
    public ElevatorState State { get; protected set; } = ElevatorState.Idle;
    public bool IsAtMaxCapacity => PassengerCount >= MaxCapacity;

    public abstract int MaxCapacity { get; }
    protected abstract int FloorTravelTimeMs { get; }

    protected ElevatorBase(int elevatorId, int startingFloor = 1)
    {
        ElevatorId = elevatorId;
        CurrentFloor = startingFloor;
    }

    public async Task MoveToFloor(int floor)
    {
        if (State == ElevatorState.Moving)
            throw new InvalidOperationException($"Elevator {ElevatorId} is already moving.");

        State = ElevatorState.Moving;
        Direction = floor > CurrentFloor ? ElevatorDirection.Up : ElevatorDirection.Down;

        while (CurrentFloor != floor)
        {
            await Task.Delay(FloorTravelTimeMs);

            if (Direction == ElevatorDirection.Up)
                CurrentFloor += 1;
            else
                CurrentFloor -= 1;
        }

        Direction = ElevatorDirection.Stationary;
        State = ElevatorState.Idle;
    }

    public void PickUpPassengers(int count)
    {
        if (PassengerCount + count > MaxCapacity)
            throw new MaxCapacityExeceededException(ElevatorId, MaxCapacity, PassengerCount + count);
        PassengerCount += count;
    }

    public void DropOffPassengers(int count)
    {
        if (count > PassengerCount)
            throw new InvalidDropOffException(ElevatorId, PassengerCount, count);

        PassengerCount -= count;
    }
}