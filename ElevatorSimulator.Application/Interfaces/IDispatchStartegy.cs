namespace ElevatorSimulator.Application.Interfaces;

using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Interfaces;

/// <summary>
/// Defines the strategy used to select which elevator services an incoming request.
/// </summary>
public interface IDispatchStrategy
{
    /// <summary>
    /// Selects the most suitable elevator for the given request from the available fleet.
    /// </summary>
    /// <param name="elevators">The full set of elevators managed by the system.</param>
    /// <param name="request">The incoming elevator request to fulfil.</param>
    /// <returns>
    /// The selected <see cref="IElevator"/>, or <see langword="null"/> if no elevator is currently available.
    /// </returns>
    IElevator? SelectElevator(IEnumerable<IElevator> elevators, ElevatorRequest request);
}
