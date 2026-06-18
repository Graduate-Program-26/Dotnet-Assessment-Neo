namespace ElevatorSimulator.Console.Rendering;

using System;
using ElevatorSimulator.Domain.Entities;
using ElevatorSimulator.Domain.Enums;
using ElevatorSimulator.Domain.Interfaces;

public static class ConsoleRenderer
{
    private const int HeaderLines = 3;
    private const int FooterLines = 2;

    /// <summary>
    /// Total number of terminal lines that the elevator status board occupies.
    /// </summary>
    public static int LineCount(int elevatorCount) => HeaderLines + elevatorCount + FooterLines;

    /// <summary>
    /// Used to 
    /// </summary>
    public static async Task RenderLoop(
        IReadOnlyList<IElevator> elevators,
        CancellationToken cancellationToken)
    {
        Console.CursorVisible = false;
        Console.Clear();

        while (!cancellationToken.IsCancellationRequested)
        {
            Console.SetCursorPosition(0, 0);
            CreateStatusBoard(elevators);

            try
            {
                await Task.Delay(200, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        Console.CursorVisible = true;
    }

    private static void CreateStatusBoard(IReadOnlyList<IElevator> elevators)
    {
        WritePaddedLine("┌─────────────────────────────────────────────────┐");
        WritePaddedLine("│           ELEVATOR SIMULATOR — STATUS           │");
        WritePaddedLine("├─────────────────────────────────────────────────┤");

        foreach (var elevator in elevators)
        {
            var typeName = GetElevatorTypeName(elevator);
            var arrow = elevator.Direction switch
            {
                ElevatorDirection.Up => "↑",
                ElevatorDirection.Down => "↓",
                _ => "─"
            };

            Console.ForegroundColor = elevator.State == ElevatorState.Moving
                ? ConsoleColor.Yellow
                : elevator.IsAtMaxCapacity
                    ? ConsoleColor.Red
                    : ConsoleColor.Green;

            WritePaddedLine(
                $"│ E{elevator.ElevatorId} │ {typeName,-9} │ Fl {elevator.CurrentFloor,3} {arrow} │ " +
                $"{elevator.State,-10} │ {elevator.PassengerCount,2}/{elevator.MaxCapacity,-2}  │");
            Console.ResetColor();
        }

        WritePaddedLine("└─────────────────────────────────────────────────┘");
        WritePaddedLine(string.Empty);
    }

    private static string GetElevatorTypeName(IElevator elevator) =>
        elevator.GetType().Name switch
        {
            nameof(PassengerElevator) => "Passenger",
            nameof(FreightElevator) => "Freight",
            nameof(HighSpeedElevator) => "HighSpeed",
            var other => other.Replace("Elevator", string.Empty)
        };

    private static void WritePaddedLine(string text)
    {
        var width = Console.WindowWidth - 1;
        Console.WriteLine(text.Length < width ? text.PadRight(width) : text);
    }
}
