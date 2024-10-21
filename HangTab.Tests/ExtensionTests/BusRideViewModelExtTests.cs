using HangTab.Extensions;
using HangTab.Models.ViewModels;

namespace HangTab.Tests.ExtensionTests;
public class BusRideViewModelExtTests
{
    [Fact]
    public void ItShouldIncrementTheBusRideCount()
    {
        // Given
        var vm = new BusRideViewModel
        {
            BusRide = { Id = 1, Total = 5 },
            BusRideWeek = { BusRides = 1, WeekNumber = 1 }
        };

        // When
        vm.AddBusRide();

        // Then
        vm.BusRide.Total.Should().Be(6);
        vm.BusRideWeek.BusRides.Should().Be(2);
    }

    [Fact]
    public void ItShouldDecrementTheBusRideCount()
    {
        // Given
        var vm = new BusRideViewModel
        {
            BusRide = { Id = 1, Total = 5 },
            BusRideWeek = { BusRides = 1, WeekNumber = 1 }
        };

        // When
        vm.UndoBusRide();

        // Then
        vm.BusRide.Total.Should().Be(4);
        vm.BusRideWeek.BusRides.Should().Be(0);
    }

    [Fact]
    public void ItShouldNotDecrementTheTotalBusRideCount()
    {
        // Given
        var vm = new BusRideViewModel
        {
            BusRide = { Id = 1, Total = 0 },
            BusRideWeek = { BusRides = 9, WeekNumber = 1 }
        };

        // When
        vm.UndoBusRide();

        // Then
        vm.BusRide.Total.Should().Be(0);
        vm.BusRideWeek.BusRides.Should().Be(9);
    }

    [Fact]
    public void ItShouldNotDecrementTheBusRideWeekCount()
    {
        // Given
        var vm = new BusRideViewModel
        {
            BusRide = { Id = 1, Total = 9 },
            BusRideWeek = { BusRides = 0, WeekNumber = 1 }
        };

        // When
        vm.UndoBusRide();

        // Then
        vm.BusRide.Total.Should().Be(9);
        vm.BusRideWeek.BusRides.Should().Be(0);
    }

    [Fact]
    public void ItShouldResetTheBusRideTotals()
    {
        // Given
        const int weekNumber = 3;
        var vm = new BusRideViewModel
        {
            BusRide = { Id = 1, Total = 9 },
            BusRideWeek = { BusRides = 5, WeekNumber = weekNumber }
        };

        // When
        vm.ResetBusRidesForWeek(weekNumber);

        // Then
        vm.BusRide.Total.Should().Be(9);
        vm.BusRideWeek.BusRides.Should().Be(0);
    }
}
