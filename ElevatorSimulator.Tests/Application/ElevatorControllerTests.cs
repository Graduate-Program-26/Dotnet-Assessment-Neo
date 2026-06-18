namespace ElevatorSimulator.Tests.Application;

using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Application.Services;
using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Interfaces;

public class ElevatorControllerTests
{
    private sealed class AlwaysFirstStrategy : IDispatchStrategy
    {
        public IElevator? SelectElevator(IEnumerable<IElevator> elevators, ElevatorRequest request)
            => elevators.FirstOrDefault(elevator => !elevator.IsAtMaxCapacity);
    }

    private static ElevatorRequest MakeRequest(int originFloor, int destinationFloor, int passengers = 1)
        => new(originFloor, destinationFloor, passengers, DateTime.UtcNow);

    [Fact]
    public async Task HandleRequest_MovesElevator_ToDestinationFloor()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        var controller = new ElevatorController(new AlwaysFirstStrategy(), new[] { elevator });

        await controller.HandleRequest(MakeRequest(originFloor: 3, destinationFloor: 7), CancellationToken.None);

        Assert.Equal(7, elevator.CurrentFloor);
    }

    [Fact]
    public async Task HandleRequest_QueuesRequest_WhenAllElevatorsUnavailable()
    {
        var elevator = new PassengerElevator(1);
        elevator.PickUpPassengers(10); // max capacity

        var controller = new ElevatorController(new AlwaysFirstStrategy(), new[] { elevator });

        await controller.HandleRequest(MakeRequest(originFloor: 1, destinationFloor: 5), CancellationToken.None);

        Assert.Equal(1, controller.PendingRequestCount);
    }

    [Fact]
    public async Task HandleRequest_ProcessesQueuedRequest_WhenElevatorBecomesAvailable()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        elevator.PickUpPassengers(10);

        var controller = new ElevatorController(new AlwaysFirstStrategy(), new[] { elevator });

        await controller.HandleRequest(MakeRequest(originFloor: 1, destinationFloor: 5), CancellationToken.None);
        Assert.Equal(1, controller.PendingRequestCount);

        elevator.DropOffPassengers(10);
        await controller.HandleRequest(MakeRequest(originFloor: 2, destinationFloor: 4), CancellationToken.None);

        Assert.Equal(0, controller.PendingRequestCount);
    }

    [Fact]
    public async Task HandleRequest_PicksUpAndDropsOffPassengers_OnFullTrip()
    {
        var elevator = new PassengerElevator(1, startingFloor: 1);
        var controller = new ElevatorController(new AlwaysFirstStrategy(), new List<IElevator> { elevator }.AsReadOnly());

        await controller.HandleRequest(MakeRequest(originFloor: 3, destinationFloor: 7, passengers: 4), CancellationToken.None);

        Assert.Equal(7, elevator.CurrentFloor);
        Assert.Equal(0, elevator.PassengerCount);
    }
}
