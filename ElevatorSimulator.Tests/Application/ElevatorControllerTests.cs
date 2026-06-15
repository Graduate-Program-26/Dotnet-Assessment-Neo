namespace ElevatorSimulator.Tests.Application;

using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Application.Services;
using ElevatorSimulator.Domain.Entities;

public class ElevatorControllerTests
{
    private static ElevatorRequest MakeRequest(int floor, int passengers = 1) 
        => new(floor, passengers, DateTime.UtcNow);

    [Fact]
    public async Task HandleRequest_MovesElevator_ToRequestedFloor()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        var controller = new ElevatorController(
            new NearestElevatorStrategy(),
            new[] { elevator });

        await controller.HandleRequest(MakeRequest(floor: 5), CancellationToken.None);

        Assert.Equal(5, elevator.CurrentFloor);
    }

    [Fact]
    public async Task HandleRequest_QueuesRequest_WhenAllElevatorsUnavailable()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(10); // max capacity

        var controller = new ElevatorController(
            new NearestElevatorStrategy(),
            new[] { elevator }
        );

        await controller.HandleRequest(MakeRequest(floor: 5), CancellationToken.None);

        Assert.Equal(1, controller.PendingRequestCount);
    }

    [Fact]
    public async Task HandleRequest_PicksUpPassengers_WhenElevatorArrives()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        var controller = new ElevatorController(
            new NearestElevatorStrategy(),
            new[] { elevator });

        await controller.HandleRequest(MakeRequest(floor: 5, passengers: 3), CancellationToken.None);

        Assert.Equal(3, elevator.PassengerCount);
    }
}