using HangTab.Extensions;
using HangTab.Models.ViewModels;

namespace HangTab.Tests.ExtensionTests;
public class BowlerViewModelExtTests
{
    [Fact]
    public void ItShouldIncrementTheHangingCount()
    {
        // Given
        var vm = new BowlerViewModel
        {
            Bowler = { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png" },
            BowlerWeek = { Id = 1, BowlerId = 1, Hangings = 1, WeekNumber = 1 },
            IsEnableSwitchBowler = false,
            IsEnableUndo = false,
            IsLowestHangs = false
        };

        // When
        vm.AddHanging();

        // Then
        vm.Bowler.TotalHangings.Should().Be(1);
        vm.BowlerWeek.Hangings.Should().Be(2);
    }

    [Fact]
    public void ItShouldDecrementTheHangingCount()
    {
        // Given
        var vm = new BowlerViewModel
        {
            Bowler = { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png", TotalHangings = 9 },
            BowlerWeek = { Id = 1, BowlerId = 1, Hangings = 4, WeekNumber = 1 },
            IsEnableSwitchBowler = false,
            IsEnableUndo = false,
            IsLowestHangs = false
        };

        // When
        vm.UndoHanging();

        // Then
        vm.Bowler.TotalHangings.Should().Be(8);
        vm.BowlerWeek.Hangings.Should().Be(3);
    }

    [Fact]
    public void ItShouldNotDecrementTheTotalHangingCount()
    {
        // Given
        var vm = new BowlerViewModel
        {
            Bowler = { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png", TotalHangings = 0 },
            BowlerWeek = { Id = 1, BowlerId = 1, Hangings = 4, WeekNumber = 1 },
            IsEnableSwitchBowler = false,
            IsEnableUndo = false,
            IsLowestHangs = false
        };

        // When
        vm.UndoHanging();

        // Then
        vm.Bowler.TotalHangings.Should().Be(0);
        vm.BowlerWeek.Hangings.Should().Be(4);
    }

    [Fact]
    public void ItShouldNotDecrementTheHangingCount()
    {
        // Given
        var vm = new BowlerViewModel
        {
            Bowler = { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png", TotalHangings = 9 },
            BowlerWeek = { Id = 1, BowlerId = 1, Hangings = 0, WeekNumber = 1 },
            IsEnableSwitchBowler = false,
            IsEnableUndo = false,
            IsLowestHangs = false
        };

        // When
        vm.UndoHanging();

        // Then
        vm.Bowler.TotalHangings.Should().Be(9);
        vm.BowlerWeek.Hangings.Should().Be(0);
    }
}
