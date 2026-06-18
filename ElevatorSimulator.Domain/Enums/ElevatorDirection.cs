namespace ElevatorSimulator.Domain.Enums;

/// <summary>
/// Describes the current direction of travel for an elevator.
/// </summary>
public enum ElevatorDirection
{
    /// <summary>The elevator is travelling upward.</summary>
    Up,

    /// <summary>The elevator is travelling downward.</summary>
    Down,

    /// <summary>The elevator is not moving.</summary>
    Stationary
}
