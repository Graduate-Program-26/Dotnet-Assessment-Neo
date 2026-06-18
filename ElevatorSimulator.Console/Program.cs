using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Application.Services;
using ElevatorSimulator.Application.Strategies;
using ElevatorSimulator.Console.Input;
using ElevatorSimulator.Console.Rendering;
using ElevatorSimulator.Console.Setup;
using Microsoft.Extensions.DependencyInjection;

var buildingConfig = BuildingConfigurator.Configure();

var services = new ServiceCollection();
services.AddSingleton<IDispatchStrategy, NearestElevatorStrategy>();
services.AddSingleton(buildingConfig.Elevators);
services.AddSingleton<ElevatorController>();
services.AddSingleton(_ => new FloorManager(buildingConfig.MinFloor, buildingConfig.MaxFloor));

var provider = services.BuildServiceProvider();
var elevatorController = provider.GetRequiredService<ElevatorController>();
var floorManager = provider.GetRequiredService<FloorManager>();

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, elevator) =>
{
    elevator.Cancel = true;
    cts.Cancel();
};

var statusLineCount = ConsoleRenderer.LineCount(buildingConfig.Elevators.Count);
var inputHandler = new InputHandler(elevatorController, floorManager, inputStartRow: statusLineCount + 1);

await Task.WhenAll(
    ConsoleRenderer.RenderLoop(elevatorController.Elevators, cts.Token),
    inputHandler.InputLoop(cts.Token)
);

Console.SetCursorPosition(0, statusLineCount + 6);
Console.WriteLine("Simulation ended. Press any key to exit.");
Console.ReadKey(intercept: true);
