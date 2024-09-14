using HangTab.Tests.TestData;
using System.Linq.Expressions;
using HangTab.Models;
using HangTab.Models.ViewModels;

namespace HangTab.Tests.DatabaseServiceTests;
public class WeeksTests : TestBase
{
    [Fact]
    public async Task ItShouldReturnAllWeeksFromTheDatabase()
    {
        // Given
        const int weekNumber = 1;
        var rides = new List<BusRide> { SimpleData.BusRide };
        var bowlers = new List<Bowler> { SimpleData.OneBowler };
        var bowlerWeeks = new List<BowlerWeek> { new() { Id = 1, BowlerId = 1, Hangings = 1, WeekNumber = 1 }, SimpleData.BowlerWeek };
        var busRideWeeks = new List<BusRideWeek> { new() { Id = 1, BusRides = 1, WeekNumber = 1 }, SimpleData.BusRideWeek };
        var expected = new List<WeekViewModel>
        {
            new() { Bowlers = bowlers, TotalBusRides = 2, TotalHangings = 1, WeekNumber = weekNumber }
        };
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(bowlers);
        A.CallTo(() => ContextFake.GetAllAsync<BowlerWeek>()).Returns(bowlerWeeks);
        A.CallTo(() => ContextFake.GetAllAsync<BusRideWeek>()).Returns(busRideWeeks);
        A.CallTo(() => ContextFake.GetAllAsync<BusRide>()).Returns(rides);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BusRideWeek, bool>>>.Ignored)).Returns(busRideWeeks);

        // When
        var actual = await DatabaseService.GetAllWeeks();

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ItShouldReturnFirstWeekAsLatestWeek()
    {
        // Given
        const int expected = 1;
        A.CallTo(() => ContextFake.GetAllAsync<BusRideWeek>()).Returns(new List<BusRideWeek>());

        // When
        var actual = await DatabaseService.GetLatestWeek();

        // Then
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task EmptyLatestWeekShouldReturnAnEmptyWeekViewModel()
    {
        // Given
        A.CallTo(() => ContextFake.GetAllAsync<BusRideWeek>()).Returns(new List<BusRideWeek>());

        // When
        var actual = await DatabaseService.GetAllWeeks();

        // Then
        actual.Should().BeEquivalentTo(new List<WeekViewModel>());
    }

    [Fact]
    public async Task EmptyBowlerListShouldAlsoReturnAnEmptyWeekViewModel()
    {
        // Given
        var busRides = new List<BusRideWeek>
        { 
            new() { Id = 1, BusRides = 0, WeekNumber = 1},
            new() { Id = 2, BusRides = 0, WeekNumber = 2}
        };

        A.CallTo(() => ContextFake.GetAllAsync<BusRideWeek>()).Returns(busRides);
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(new List<Bowler>());

        // When
        var actual = await DatabaseService.GetAllWeeks();

        // Then
        actual.Should().BeEquivalentTo(new List<WeekViewModel>());
    }

    [Fact]
    public async Task EmptyBowlerWeekListShouldAlsoReturnAnEmptyWeekViewModel()
    {
        // Given
        var busRides = new List<BusRideWeek>
        { 
            new() { Id = 1, BusRides = 0, WeekNumber = 1},
            new() { Id = 2, BusRides = 0, WeekNumber = 2}
        };

        A.CallTo(() => ContextFake.GetAllAsync<BusRideWeek>()).Returns(busRides);
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(SimpleData.ListOfFiveBowlers);
        A.CallTo(() => ContextFake.GetAllAsync<BowlerWeek>()).Returns(new List<BowlerWeek>());

        // When
        var actual = await DatabaseService.GetAllWeeks();

        // Then
        actual.Should().BeEquivalentTo(new List<WeekViewModel>());
    }
}
