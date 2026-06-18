namespace ElevatorSimulator.Application.Models;

/// <summary>
/// Represents a passenger request submitted to the elevator dispatch system.
/// </summary>
/// <param name="OriginFloor">The floor on which passengers are waiting to board.</param>
/// <param name="DestinationFloor">The floor to which passengers wish to travel.</param>
/// <param name="PassengerCount">The number of passengers making the request.</param>
/// <param name="RequestedAt">The UTC timestamp at which the request was submitted.</param>
public record ElevatorRequest(int OriginFloor, int DestinationFloor, int PassengerCount, DateTime RequestedAt);
