namespace ElevatorSimulator.Application.Interfaces;

using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Domain.Interfaces;

public interface IDispatchStrategy
{
    //nullable because it might not find an available elevator 
    IElevator? SelectElevator(IEnumerable<IElevator> elevators, ElevatorRequest request);
}