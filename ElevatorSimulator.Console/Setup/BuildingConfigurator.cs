namespace ElevatorSimulator.Console.Setup;

using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Interfaces;
using System;

internal sealed record BuildingConfiguration(
    int MinFloor,
    int MaxFloor,
    IReadOnlyList<IElevator> Elevators);

internal enum ElevatorTypeChoice { Passenger, Freight, HighSpeed }

internal static class BuildingConfigurator
{
    private const int MinFloors = 2;
    private const int MaxFloors = 100;
    private const int MinElevators = 1;
    private const int MaxElevators = 10;

    public static BuildingConfiguration Configure()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║        ELEVATOR SIMULATOR  —  SETUP          ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();

        var floors = ReadValidatedInt(
            $"Number of floors ({MinFloors}–{MaxFloors}): ",
            validNumber => validNumber is >= MinFloors and <= MaxFloors);

        var elevatorCount = ReadValidatedInt(
            $"Number of elevators ({MinElevators}–{MaxElevators}): ",
            validNumber => validNumber is >= MinElevators and <= MaxElevators);

        Console.WriteLine();

        var elevators = new List<IElevator>(elevatorCount);
        for (int i = 1; i <= elevatorCount; i++)
        {
            var choice = ReadElevatorType(i);
            IElevator elevator = choice switch
            {
                ElevatorTypeChoice.Freight => new FreightElevator(i),
                ElevatorTypeChoice.HighSpeed => new HighSpeedElevator(i),
                _ => new PassengerElevator(i),
            };
            elevators.Add(elevator);
        }

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Starting simulation... Press Ctrl+C to exit.");
        Console.ResetColor();

        return new BuildingConfiguration(MinFloor: 1, MaxFloor: floors, elevators.AsReadOnly());
    }

    private static int ReadValidatedInt(string systemPrompt, Func<int, bool> isValidInt)
    {
        while (true)
        {
            Console.Write(systemPrompt);
            var userInput = Console.ReadLine() ?? string.Empty;
            if (int.TryParse(userInput, out var validNumber) && isValidInt(validNumber))
                return validNumber;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ! Invalid — try again.");
            Console.ResetColor();
        }
    }

    private static ElevatorTypeChoice ReadElevatorType(int elevatorId)
    {
        while (true)
        {
            Console.Write($"  Elevator {elevatorId} — [P] Passenger  [F] Freight  [H] High-Speed: ");
            var userInput = (Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

            ElevatorTypeChoice? selectedElevator = userInput switch
            {
                "P" or "PASSENGER" => ElevatorTypeChoice.Passenger,
                "F" or "FREIGHT" => ElevatorTypeChoice.Freight,
                "H" or "HIGH-SPEED" or "HIGHSPEED" or "HS" => ElevatorTypeChoice.HighSpeed,
                _ => null,
            };

            if (selectedElevator is not null)
                return selectedElevator.Value;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    ! Enter P, F, or H.");
            Console.ResetColor();
        }
    }
}
