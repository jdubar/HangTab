using HangTab.Models;

namespace HangTab.Tests.TestData;

public static class SimpleData
{
    public static Bowler OneBowler { get; } = new() { Id = 1, FirstName = "Jason", LastName = "Smith", ImageUrl = "123.png", IsHidden = false, IsSub = false, TotalHangings = 1 };
    public static Bowler OneSubBowler { get; } = new() { Id = 1, FirstName = "Nick", LastName = "Bertus", ImageUrl = "uh-oh.png", IsSub = true };
    public static BusRide BusRide { get; } = new() { Id = 1, Total = 3 };
    public static BowlerWeek BowlerWeek { get; } = new() { Id = 2, BowlerId = 1, Hangings = 3, WeekNumber  = 2 };
    public static BusRideWeek BusRideWeek { get; } = new() { Id = 2, BusRides = 2, WeekNumber = 2 };
}