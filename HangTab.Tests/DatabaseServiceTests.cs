using HangTab.Data;
using HangTab.Models;
using HangTab.Services.Impl;

using System.Linq.Expressions;

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
    public async Task ItShouldCheckIfBowlerExists()
    {
        // Given
        var bowler = new Bowler { FirstName = "Donnie", LastName = "George" };
        var bowlers = new List<Bowler> { bowler };
        const bool expected = true;

        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(bowlers);

        // When
        var actual = await DatabaseService.IsBowlerExists(bowler);

        // Then
        actual.Should().Be(expected);
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