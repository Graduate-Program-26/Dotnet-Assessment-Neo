namespace ElevatorSimulator.Tests.Application;

using ElevatorSimulator.Application.Services;
using ElevatorSimulator.Domain.Exceptions;

public class FloorManagerTests
{
    [Fact]
    public void ValidateFloor_Throws_WhenBelowMinimumFloor()
    {
        var floorManager = new FloorManager(minFloor: 1, maxFloor: 20);
        Assert.Throws<InvalidFloorException>(() => floorManager.ValidateFloor(0));
    }

    [Fact]
    public void ValidateFloor_Throws_WhenAboveMaximumFloor()
    {
        var floorManager = new FloorManager(minFloor: 1, maxFloor: 20);
        Assert.Throws<InvalidFloorException>(() => floorManager.ValidateFloor(21));
    }

    [Fact]
    public void ValidateFloor_DoesNotThrow_ForValidFloor()
    {
        var floorManager = new FloorManager(minFloor: 1, maxFloor: 20);
        floorManager.ValidateFloor(10);
    }
}