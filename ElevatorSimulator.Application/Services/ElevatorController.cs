namespace ElevatorSimulator.Application.Services;

using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Interfaces;

public sealed class ElevatorController
{
    private readonly IDispatchStrategy _strategy;
    private readonly IReadOnlyList<IElevator> _elevators;
    private readonly Queue<ElevatorRequest> _pendingRequests = new();

    public IReadOnlyList<IElevator> Elevators => _elevators;
    public int PendingRequestCount => _pendingRequests.Count;

    public ElevatorController(IDispatchStrategy strategy, IEnumerable<IElevator> elevators)
    {
        _strategy = strategy;
        _elevators = elevators.ToList();
    }

    public async Task HandleRequest(ElevatorRequest request, CancellationToken cancellationToken)
    {
        var elevator = _strategy.SelectElevator(_elevators, request);

        if (elevator is null)
        {
            _pendingRequests.Enqueue(request);
            return;
        }

        await elevator.MoveToFloor(request.OriginFloor);
        elevator.PickUpPassengers(request.PassengerCount);
        await elevator.MoveToFloor(request.DestinationFloor);
        elevator.DropOffPassengers(request.PassengerCount);

        if (_pendingRequests.TryDequeue(out var next))
            await HandleRequest(next, cancellationToken);
    }
}