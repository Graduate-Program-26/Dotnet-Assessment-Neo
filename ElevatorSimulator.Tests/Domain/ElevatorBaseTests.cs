namespace ElevatorSimulator.Tests.Domain;

using ElevatorSimulator.Domain.Enums;
using ElevatorSimulator.Domain.Exceptions;
using ElevatorSimulator.Domain.Entities;

public class ElevatorBaseTests
{
    [Fact]
    public void PickUpPassengers_ThrowsMaxCapacityExceeded_WhenPassengersOverLimit()
    {
        var elevator = new PassengerElevator(1);
        Assert.Throws<MaxCapacityExeceededException>(
            () => elevator.PickUpPassengers(11)
        );
    }

    [Fact]
    public void PickUpPassengers_Succeeds_WhenAtExactCapacity()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(10);
        Assert.Equal(10, elevator.PassengerCount);
    }

    [Fact]
    public void IsAtCapacity_ReturnsTrue_WhenFull()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(10);
        Assert.True(elevator.IsAtMaxCapacity);
    }

    [Fact]
    public async Task MoveToFloor_UpdatesCurrentFloor()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        await elevator.MoveToFloor(3);
        Assert.Equal(3, elevator.CurrentFloor);
    }

    [Fact]
    public async Task MoveToFloor_SetsDirectionUp_WhenTargetIsHigher()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        var moveTask = elevator.MoveToFloor(5);
        Assert.Equal(ElevatorDirection.Up, elevator.Direction);
        await moveTask;
    }

    [Fact]
    public async Task MoveToFloor_SetsStateToIdle_WhenArrived()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        await elevator.MoveToFloor(2);
        Assert.Equal(ElevatorState.Idle, elevator.State);
    }

    [Fact]
    public async Task MoveToFloor_Throws_WhenAlreadyMoving()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);

        var move = elevator.MoveToFloor(10);

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => elevator.MoveToFloor(5)
        );
    }

    [Fact]
    public async Task MoveToFloor_SetsDirectionDown_WhenTargetIsLower()
    {
        var elevator = new PassengerElevator(1, startingFloor: 5);
        var moveTask = elevator.MoveToFloor(1);
        Assert.Equal(ElevatorDirection.Down, elevator.Direction);
        await moveTask;
    }

    [Fact]
    public async Task MoveToFloor_SetsDirectionStationary_WhenArrived()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        await elevator.MoveToFloor(3);
        Assert.Equal(ElevatorDirection.Stationary, elevator.Direction);
    }

    [Fact]
    public void PickUpPassengers_Throws_WhenPartiallyFullAndOverCapacity()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(5);
        Assert.Throws<MaxCapacityExeceededException>(
            () => elevator.PickUpPassengers(6)
        );
    }

    [Fact]
    public void DropOffPassengers_DecreasesPassengerCount()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(5);
        elevator.DropOffPassengers(3);
        Assert.Equal(2, elevator.PassengerCount);
    }

    [Fact]
    public void DropOffPassengers_DoesNotGoBelowZero()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(3);
        elevator.DropOffPassengers(10);
        Assert.Equal(0, elevator.PassengerCount);
    }
}