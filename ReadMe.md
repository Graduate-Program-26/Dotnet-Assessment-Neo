# Elevator Simulator

A C# console application that simulates elevator dispatch and movement in a configurable multi-floor building.

---

## Features

- **Three elevator types** — Passenger, Freight, and High-Speed, each with distinct capacity and speed characteristics.
- **Nearest-elevator dispatch** — requests are routed to the closest idle, non-full elevator.
- **Request queuing** — requests that cannot be fulfilled immediately are held and retried automatically once an elevator becomes free.
- **Live status board** — a real-time terminal display updates every 200 ms showing each elevator's floor, direction, state, and occupancy.
- **Interactive input** — enter an origin floor, destination floor, and passenger count at any time during the simulation.
- **Graceful shutdown** — press `Ctrl+C` to cancel all in-flight trips and exit cleanly.

---

## Project Structure

```
ElevatorSimulator.Domain/          # Core entities, interfaces, enums, and domain exceptions
  Entities/
    ElevatorBase.cs                # Abstract base: movement loop, pick-up/drop-off logic
    PassengerElevator.cs           # Capacity 10 · 500 ms/floor
    FreightElevator.cs             # Capacity 20 · 800 ms/floor
    HighSpeedElevator.cs           # Capacity  6 · 200 ms/floor
  Interfaces/
    IElevator.cs
  Enums/
    ElevatorDirection.cs           # Up | Down | Stationary
    ElevatorState.cs               # Idle | Moving | DoorsOpen
  Exceptions/
    InvalidFloorException.cs
    InvalidDropOffException.cs
    MaxCapacityExceededException.cs

ElevatorSimulator.Application/     # Use-cases, services, and dispatch strategies
  Models/
    ElevatorRequest.cs             # Record: OriginFloor, DestinationFloor, PassengerCount, RequestedAt
  Interfaces/
    IDispatchStrategy.cs
  Services/
    ElevatorController.cs          # Dispatches requests; manages the pending-request queue
    FloorManager.cs                # Validates floor numbers against the building range
  Strategies/
    NearestElevatorStrategy.cs     # Selects the closest idle, non-full elevator

ElevatorSimulator.Console/         # Presentation layer
  Setup/
    BuildingConfigurator.cs        # Interactive setup wizard (floors, elevator count & types)
  Rendering/
    ConsoleRenderer.cs             # Real-time status board rendered to the terminal
  Input/
    InputHandler.cs                # Async input loop; collects and submits elevator requests
  Program.cs                       # Composition root; wires DI and starts the simulation

ElevatorSimulator.Tests/           # Unit tests
ElevatorSimulator.Infrastructure/  # Reserved for future infrastructure concerns
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Run

```bash
dotnet run --project ElevatorSimulator.Console
```

### Test

```bash
dotnet test
```

---

## How It Works

1. **Setup** — on launch, a wizard asks for the number of floors (2–100) and elevators (1–10). Each elevator is assigned a type (Passenger / Freight / High-Speed).
2. **Simulation** — `ConsoleRenderer` refreshes the status board in a background loop while `InputHandler` accepts requests from the user.
3. **Dispatch** — `ElevatorController` calls `NearestElevatorStrategy` to find the best elevator. If none is available the request is queued in a `ConcurrentQueue` and retried after every completed trip.
4. **Movement** — `ElevatorBase.MoveToFloor` advances one floor at a time with a configurable `Task.Delay`, updating `CurrentFloor`, `Direction`, and `State` as it goes.
5. **Exit** — press `Ctrl+C` to cancel the `CancellationToken` shared across all loops.

---

## Elevator Types

| Type        | Max Capacity | Speed (ms / floor) |
|-------------|:------------:|:------------------:|
| Passenger   | 10           | 500                |
| Freight     | 20           | 800                |
| High-Speed  | 6            | 200                |

---

## Architecture

The solution follows a **Clean Architecture** layering:

- **Domain** has no dependencies on other projects.
- **Application** depends only on Domain.
- **Console** depends on Application and Domain, and uses `Microsoft.Extensions.DependencyInjection` as the composition root.
- **Infrastructure** is a placeholder for future persistence or external integrations.
