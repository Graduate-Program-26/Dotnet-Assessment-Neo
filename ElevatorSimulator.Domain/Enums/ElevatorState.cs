namespace ElevatorSimulator.Domain.Enums;

/// <summary>
/// Represents the operational state of an elevator.
/// </summary>
public enum ElevatorState
{
    /// <summary>The elevator doors are open and passengers may board or alight.</summary>
    DoorsOpen,

    /// <summary>The elevator is in transit between floors.</summary>
    Moving,

    /// <summary>The elevator is stationary with doors closed, awaiting a request.</summary>
    Idle
}
