namespace ElevatorSimulator.Tests.Application;

using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Application.Strategies;
using ElevatorSimulator.Domain.Entities;

public class DispatchStrategyTests
{
    private static ElevatorRequest MakeRequest(int originFloor, int passengers = 1)
        => new(originFloor, originFloor + 1, passengers, DateTime.UtcNow);

    [Fact]
    public void SelectElevator_ReturnsNearest_WhenMultipleIdle()
    {
        var elevators = new[]
        {
            new PassengerElevator(1, startingFloor: 10),
            new PassengerElevator(2, startingFloor: 3),
        };
        var strategy = new NearestElevatorStrategy();

        var selected = strategy.SelectElevator(elevators, MakeRequest(originFloor: 5));

        Assert.Equal(2, selected!.ElevatorId); 
    }

    [Fact]
    public void SelectElevator_SkipsFullElevator()
    {
        var full = new PassengerElevator(1, startingFloor: 1);
        full.PickUpPassengers(10);

        var available = new PassengerElevator(2, startingFloor: 5);
        var strategy = new NearestElevatorStrategy();

        var selected = strategy.SelectElevator(new[] { full, available }, MakeRequest(originFloor: 1));

        Assert.Equal(2, selected!.ElevatorId);
    }

    [Fact]
    public void SelectElevator_ReturnsNull_WhenAllFull()
    {
        var elevators = new[] { new PassengerElevator(1) };
        elevators[0].PickUpPassengers(10);
        var strategy = new NearestElevatorStrategy();

        var selected = strategy.SelectElevator(elevators, MakeRequest(originFloor: 5));

        Assert.Null(selected);
    }

    [Fact]
    public async Task SelectElevator_SkipsMovingElevator()
    {
        var moving = new PassengerElevator(1, startingFloor: 1);
        var idle = new PassengerElevator(2, startingFloor: 5);
        var strategy = new NearestElevatorStrategy();

        var moveTask = moving.MoveToFloor(10);

        var selected = strategy.SelectElevator(new[] { moving, idle }, MakeRequest(originFloor: 1));

        Assert.Equal(2, selected!.ElevatorId);
        await moveTask;
    }
}