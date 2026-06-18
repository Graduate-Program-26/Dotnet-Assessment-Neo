namespace ElevatorSimulator.Application.Services;

using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Interfaces;
using System.Collections.Concurrent;

/// <summary>
/// Orchestrates elevator dispatching by delegating requests to an <see cref="IDispatchStrategy"/>
/// and managing a queue of requests that could not be immediately fulfilled.
/// </summary>
public sealed class ElevatorController
{
    private readonly IDispatchStrategy _strategy;
    private readonly IReadOnlyList<IElevator> _elevators;

    private readonly ConcurrentQueue<ElevatorRequest> _pendingRequests = new();

    /// <summary>Gets the list of elevators managed by this controller.</summary>
    public IReadOnlyList<IElevator> Elevators => _elevators;

    /// <summary>Gets the number of requests currently waiting for an available elevator.</summary>
    public int PendingRequestCount => _pendingRequests.Count;

    /// <summary>
    /// Initializes a new <see cref="ElevatorController"/> with the specified dispatch strategy and elevator fleet.
    /// </summary>
    /// <param name="strategy">The strategy used to select elevators for incoming requests.</param>
    /// <param name="elevators">The fixed set of elevators available in the building.</param>
    public ElevatorController(IDispatchStrategy strategy, IReadOnlyList<IElevator> elevators)
    {
        _strategy = strategy;
        _elevators = elevators;
    }

    /// <summary>
    /// Dispatches the given request to an available elevator, or enqueues it if none is currently free.
    /// </summary>
    /// <param name="request">The elevator request to handle.</param>
    /// <param name="cancellationToken">A token that can cancel the dispatch operation.</param>
    /// <returns>A task representing the asynchronous dispatch operation.</returns>
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
