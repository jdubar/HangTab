using HangTab.Data;
using HangTab.Models;
using HangTab.Services.Impl;

using System.Linq.Expressions;
using HangTab.Tests.TestData;
using HangTab.ViewModels;

namespace HangTab.Tests;

public class DatabaseServiceTests
{
    private IDatabaseContext ContextFake { get; }
    private DatabaseService DatabaseService { get; }

    public DatabaseServiceTests()
    {
        ContextFake = A.Fake<IDatabaseContext>();
        DatabaseService = new DatabaseService(ContextFake);
    }

    [Fact]
    public async Task ItShouldAddMainBowlerToTheDatabase()
    {
        // Given
        A.CallTo(() => ContextFake.AddItemAsync(A<Bowler>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.AddBowler(SimpleData.OneBowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldDeleteTheBowlerFromTheDatabase()
    {
        // Given
        A.CallTo(() => ContextFake.DeleteItemByIdAsync<Bowler>(A<int>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.DeleteBowler(SimpleData.OneBowler.Id);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldDeleteAllTables()
    {
        // Given
        A.CallTo(() => ContextFake.DropTableAsync<Bowler>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BowlerWeek>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BusRide>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BusRideWeek>()).Returns(true);

        // When
        var actual = await DatabaseService.DropAllTables();

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldCheckIfBowlerExists()
    {
        // Given
        var bowlers = new List<Bowler> { SimpleData.OneBowler };
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(bowlers);

        // When
        var actual = await DatabaseService.IsBowlerExists(SimpleData.OneBowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldReturnAllBowlersFromTheDatabase()
    {
        // Given
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(SimpleData.ListOfFiveBowlers);

        // When
        var actual = await DatabaseService.GetAllBowlers();

        // Then
        actual.Should().BeEquivalentTo(SimpleData.ListOfFiveBowlers);
    }

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
    public async Task ItShouldReturnTheBusRideViewModel()
    {
        // Given
        const int weekNumber = 1;
        var ride = new BusRide { Id = 1, Total = 3 };
        var week = new BusRideWeek { Id = 1, BusRides = 2, WeekNumber = weekNumber };
        var rides = new List<BusRide> { ride };
        var weeks = new List<BusRideWeek> { week };
        var expected = new BusRideViewModel { BusRide = ride, BusRideWeek = week };

        A.CallTo(() => ContextFake.GetAllAsync<BusRide>()).Returns(rides);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BusRideWeek, bool>>>.Ignored)).Returns(weeks);

        // When
        var actual = await DatabaseService.GetBusRideViewModelByWeek(weekNumber);

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
}