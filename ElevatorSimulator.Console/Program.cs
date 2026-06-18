using ElevatorSimulator.Application.Interfaces;
using ElevatorSimulator.Application.Services;
using ElevatorSimulator.Application.Strategies;
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


