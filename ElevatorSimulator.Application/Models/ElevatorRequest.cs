namespace ElevatorSimulator.Application.Models;

public record ElevatorRequest(int OriginFloor, int DestinationFloor, int PassengerCount, DateTime RequestedAt);