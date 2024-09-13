using System.Linq.Expressions;
using FakeItEasy;

using HangTab.Models;
using HangTab.Models.ViewModels;

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
    public async Task ItShouldReturnAnEmptyBusRideViewModel()
    {
        // Given
        var week = 3;
        var expected = new BusRideViewModel();
        var rides = new List<BusRide>();
        A.CallTo(() => ContextFake.GetAllAsync<BusRide>()).Returns(rides);

        // When
        var actual = await DatabaseService.GetBusRideViewModelByWeek(week);

        // Then
        actual.Should().BeEquivalentTo(expected);
    }
}
