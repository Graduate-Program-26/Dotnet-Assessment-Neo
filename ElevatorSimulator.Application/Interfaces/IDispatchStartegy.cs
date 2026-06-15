namespace ElevatorSimulator.Application.Interfaces;

using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Interfaces;

public interface IDispatchStrategy
{
    IElevator SelectElevator(IEnumerable<IElevator> elevators, ElevatorRequest request);
}