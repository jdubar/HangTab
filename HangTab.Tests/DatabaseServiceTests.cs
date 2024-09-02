using HangTab.Data;
using HangTab.Models;
using HangTab.Services.Impl;

using System.Linq.Expressions;
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
        var bowler = new Bowler { Id = 1, FirstName = "Donnie", LastName = "George", ImageUrl = "test.png" };

        A.CallTo(() => ContextFake.AddItemAsync(A<Bowler>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.AddBowler(bowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldDeleteTheBowlerFromTheDatabase()
    {
        // Given
        var bowler = new Bowler { Id = 1, FirstName = "Donnie", LastName = "George", ImageUrl = "test.png" };

        A.CallTo(() => ContextFake.DeleteItemByIdAsync<Bowler>(A<int>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.DeleteBowler(bowler.Id);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldCheckIfBowlerExists()
    {
        // Given
        var bowler = new Bowler { FirstName = "Donnie", LastName = "George" };
        var bowlers = new List<Bowler> { bowler };

        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(bowlers);

        // When
        var actual = await DatabaseService.IsBowlerExists(bowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldReturnAllBowlersFromTheDatabase()
    {
        // Given
        var bowlers = new List<Bowler>()
        {
            new() { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png" },
            new() { Id = 1, FirstName = "Jason", LastName = "Smith", ImageUrl = "123.png" },
            new() { Id = 1, FirstName = "Kenny", LastName = "Smith", ImageUrl = "daddy.png" },
            new() { Id = 1, FirstName = "Nick", LastName = "Bertus", ImageUrl = "uh-oh.png", IsSub = true },
            new() { Id = 1, FirstName = "Mike Jr.", LastName = "Fizzle", ImageUrl = "happy.png", IsSub = true, IsHidden = true},
        };

        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(bowlers);

        // When
        var actual = await DatabaseService.GetAllBowlers();

        // Then
        actual.Should().BeEquivalentTo(bowlers);
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