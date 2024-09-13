using HangTab.Models;
using HangTab.Models.ViewModels;

using System.Linq.Expressions;

namespace HangTab.Tests.DatabaseServiceTests;
public class BusRideTests : TestBase
{
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
    public async Task ItShouldReturnAnEmptyBusRideViewModelBecauseAddBusRideFails()
    {
        // Given
        const int week = 1;
        var expected = new BusRideViewModel();
        A.CallTo(() => ContextFake.GetAllAsync<BusRide>()).Returns(new List<BusRide>());
        A.CallTo(() => ContextFake.AddItemAsync(new BusRide())).Returns(false);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BusRideWeek, bool>>>.Ignored)).Returns(new List<BusRideWeek>());
        A.CallTo(() => ContextFake.AddItemAsync(new BusRideWeek())).Returns(false);

        // When
        var actual = await DatabaseService.GetBusRideViewModelByWeek(week);

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ItShouldReturnAnEmptyBusRideViewModelBecauseAddBusRideWeekFails()
    {
        // Given
        const int week = 1;
        var oneBusRide = new List<BusRide> { new() { Id = 1, Total = 1 } };
        var expected = new BusRideViewModel();
        A.CallTo(() => ContextFake.GetAllAsync<BusRide>()).Returns(oneBusRide);
        A.CallTo(() => ContextFake.AddItemAsync(new BusRide())).Returns(true);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BusRideWeek, bool>>>.Ignored)).Returns(new List<BusRideWeek>());
        A.CallTo(() => ContextFake.AddItemAsync(new BusRideWeek())).Returns(false);

        // When
        var actual = await DatabaseService.GetBusRideViewModelByWeek(week);

        // Then
        actual.Should().BeEquivalentTo(expected);
    }
}
