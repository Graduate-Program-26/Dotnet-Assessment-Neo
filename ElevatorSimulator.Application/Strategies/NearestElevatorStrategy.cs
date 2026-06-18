namespace ElevatorSimulator.Application.Strategies;

using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Enums;
using ElevatorSimulator.Domain.Interfaces;

public sealed class NearestElevatorStrategy : IDispatchStrategy
{
    public IElevator? SelectElevator(IEnumerable<IElevator> elevators, ElevatorRequest request)
    {
        return elevators
            .Where(elavator => !elavator.IsAtMaxCapacity && elavator.State == ElevatorState.Idle)
            .OrderBy(elavator => Math.Abs(elavator.CurrentFloor - request.OriginFloor))
            .FirstOrDefault();
    }
}