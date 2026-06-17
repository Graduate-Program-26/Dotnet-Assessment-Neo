namespace ElevatorSimulator.Application.Services;

using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Interfaces;
using System.Collections.Concurrent;

public sealed class ElevatorController
{
    private readonly IDispatchStrategy _strategy;
    private readonly IReadOnlyList<IElevator> _elevators;

    private readonly ConcurrentQueue<ElevatorRequest> _pendingRequests = new();

    public IReadOnlyList<IElevator> Elevators => _elevators;
    public int PendingRequestCount => _pendingRequests.Count;

    public ElevatorController(IDispatchStrategy strategy, IReadOnlyList<IElevator> elevators)
    {
        _strategy = strategy;
        _elevators = elevators;
    }

    public Task HandleRequest(ElevatorRequest request, CancellationToken cancellationToken)
    {
        var elevator = _strategy.SelectElevator(_elevators, request);

        if (elevator is null)
        {
            _pendingRequests.Enqueue(request);
            return Task.CompletedTask;
        }

        return ExecuteElevatorTrip(elevator, request, cancellationToken);
    }

    private async Task ExecuteElevatorTrip(IElevator elevator, ElevatorRequest request, CancellationToken cancellationToken)
    {
        elevator.PickUpPassengers(request.PassengerCount);
        await elevator.MoveToFloor(request.OriginFloor);
        await elevator.MoveToFloor(request.DestinationFloor);
        elevator.DropOffPassengers(request.PassengerCount);

        EmptyPendingRequests(cancellationToken);
    }

    private void EmptyPendingRequests(CancellationToken cancellationToken)
    {
        var requestsSnapshot = new List<ElevatorRequest>();
        while (_pendingRequests.TryDequeue(out var queued))
            requestsSnapshot.Add(queued);

        foreach (var queuedRequest in requestsSnapshot)
        {
            var elevator = _strategy.SelectElevator(_elevators, queuedRequest);
            if (elevator is not null)
                _ = ExecuteElevatorTrip(elevator, queuedRequest, cancellationToken);
            else
                _pendingRequests.Enqueue(queuedRequest);
        }
    }
}