namespace ElevatorSimulator.Console.Input;

using System;
using System.Text;
using ElevatorSimulator.Application.Models;
using ElevatorSimulator.Application.Services;
using ElevatorSimulator.Domain.Exceptions;

public sealed class InputHandler(ElevatorController controller, FloorManager floorManager, int inputStartRow)
{
    private const int InputRows = 5;

    public async Task InputLoop(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested)
            {
                ClearInputArea();

                var origin = await ReadFloor(row: inputStartRow, systemPrompt: "Origin floor (0 to quit):  ", ct);
                if (origin is null) break;

                var destination = await ReadFloor(row: inputStartRow + 1, systemPrompt: "Destination floor:         ", ct);
                if (destination is null) break;

                var passengers = await ReadPassengers(row: inputStartRow + 2, systemPrompt: "Passengers:                ", ct);
                if (passengers is null) break;

                var request = new ElevatorRequest(origin.Value, destination.Value, passengers.Value, DateTime.UtcNow);
                _ = controller.HandleRequest(request, ct);

                WriteFeedback(
                    $"Request accepted. {passengers} passenger(s) from floor {origin} to floor {destination}.",
                    ConsoleColor.Cyan);

                await Task.Delay(1500, ct);
            }
        }
        catch (OperationCanceledException) { }
    }

    private async Task<int?> ReadFloor(int row, string systemPrompt, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            ClearRow(row);
            Console.SetCursorPosition(0, row);
            Console.Write(systemPrompt);
            var userInput = await ReadLineAsync(row, systemPrompt.Length, ct);

            if (!int.TryParse(userInput, out var value))
            {
                ShowRowError(row, systemPrompt, "Enter a whole number");
                await Task.Delay(900, ct);
                continue;
            }

            if (value == 0)
                return null;

            try
            {
                floorManager.ValidateFloor(value);
                return value;
            }
            catch (InvalidFloorException ex)
            {
                ShowRowError(row, systemPrompt, ex.Message);
                await Task.Delay(900, ct);
            }
        }

        return null;
    }

    private static async Task<int?> ReadPassengers(int row, string systemPrompt, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            ClearRow(row);
            Console.SetCursorPosition(0, row);
            Console.Write(systemPrompt);
            var userInput = await ReadLineAsync(row, systemPrompt.Length, ct);

            if (int.TryParse(userInput, out var value) && value > 0)
                return value;

            ShowRowError(row, systemPrompt, "enter a positive whole number");
            await Task.Delay(900, ct);
        }

        return null;
    }

    private static async Task<string> ReadLineAsync(int row, int column, CancellationToken ct)
    {
        var buffer = new StringBuilder();
        while (!ct.IsCancellationRequested)
        {
            if (!Console.KeyAvailable)
            {
                await Task.Delay(20, ct);
                continue;
            }

            var key = Console.ReadKey(intercept: true);

            if (key.Key == ConsoleKey.Enter)
                break;

            if (key.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length > 0)
                    buffer.Remove(buffer.Length - 1, 1);
            }
            else if (!char.IsControl(key.KeyChar))
            {
                buffer.Append(key.KeyChar);
            }

            var inputWidth = Console.WindowWidth - 1 - column;
            Console.SetCursorPosition(column, row);
            Console.Write(buffer.ToString().PadRight(inputWidth));
        }

        return buffer.ToString();
    }

    private static void ShowRowError(int row, string systemPrompt, string hint)
    {
        Console.SetCursorPosition(0, row);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{systemPrompt}[! {hint}]".PadRight(Console.WindowWidth - 1));
        Console.ResetColor();
    }

    private void WriteFeedback(string message, ConsoleColor color)
    {
        Console.SetCursorPosition(0, inputStartRow + 3);
        Console.ForegroundColor = color;
        Console.Write(message.PadRight(Console.WindowWidth - 1));
        Console.ResetColor();
    }

    private void ClearInputArea()
    {
        for (var i = 0; i < InputRows; i++)
            ClearRow(inputStartRow + i);
    }

    private static void ClearRow(int row)
    {
        Console.SetCursorPosition(0, row);
        Console.Write(string.Empty.PadRight(Console.WindowWidth - 1));
    }
}
