namespace ElevatorSimulator.Application.Models;

public record ElevatorRequest(int OriginFloor, int PassengerCount, DateTime RequestedAt);