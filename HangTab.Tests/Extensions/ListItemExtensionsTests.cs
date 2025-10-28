using HangTab.Extensions;
using HangTab.Models;
using HangTab.ViewModels.Items;
using HangTab.ViewModels.Items.Interfaces;

namespace HangTab.Tests.Extensions;

public class ListItemExtensionsTests
{
    private class TestBowler : ILowestHangCountBowler
    {
        public int Id { get; set; }
        public bool IsSub { get; set; }
        public int HangCount { get; set; }
        public bool HasLowestHangCount { get; set; }
    }

    [Fact]
    public void SetLowestBowlerHangCount_SetsCorrectly_ForNonSubs()
    {
        // Arrange
        var bowlers = new List<TestBowler>
        {
            new() { Id = 1, IsSub = false, HangCount = 2 },
            new() { Id = 2, IsSub = false, HangCount = 5 },
            new() { Id = 3, IsSub = false, HangCount = 2 },
            new() { Id = 4, IsSub = true,  HangCount = 1 }
        };

        // Act
        bowlers.SetLowestBowlerHangCount();

        // Assert
        Assert.True(bowlers[0].HasLowestHangCount);
        Assert.False(bowlers[1].HasLowestHangCount);
        Assert.True(bowlers[2].HasLowestHangCount);
        Assert.False(bowlers[3].HasLowestHangCount); // sub is ignored
    }

    [Fact]
    public void SetLowestBowlerHangCount_EmptyList_DoesNotThrow()
    {
        // Arrange
        var bowlers = new List<TestBowler>();

        // Act & Assert
        bowlers.SetLowestBowlerHangCount();

        Assert.Empty(bowlers);
    }

    [Fact]
    public void SetBowlerHangSumByWeeks_SetsHangCountCorrectly()
    {
        // Arrange
        var bowler1 = new BowlerListItemViewModel{ Id = 1, Name = "A", IsSub = true };
        var bowler2 = new BowlerListItemViewModel{ Id = 2, Name = "B", IsSub = false };
        var weeks = new List<Week>
        {
            new()
            {
                Bowlers =
                [
                    new() { PersonId = 1, SubId = null, Status = Enums.Status.Active, HangCount = 2 },
                    new() { PersonId = 2, SubId = null, Status = Enums.Status.Active, HangCount = 3 }
                ]
            },
            new()
            {
                Bowlers =
                [
                    new() { PersonId = 1, SubId = null, Status = Enums.Status.Active, HangCount = 1 },
                    new() { PersonId = 2, SubId = null, Status = Enums.Status.Active, HangCount = 4 }
                ]
            }
        };
        var bowlers = new List<BowlerListItemViewModel> { bowler1, bowler2 };

        // Act
        bowlers.SetBowlerHangSumByWeeks(weeks);

        // Assert
        Assert.Equal(3, bowler1.HangCount); // 2 + 1
        Assert.Equal(7, bowler2.HangCount); // 3 + 4
    }

    [Fact]
    public void SetBowlerHangSumByWeeks_WithSubs_SumsCorrectly()
    {
        // Arrange
        var subId = 10;
        var hangCount = 5;
        var bowler = new BowlerListItemViewModel{ Id = subId, Name = "Super Sub", IsSub = true };
        var weeks = new List<Week>
        {
            new()
            {
                Bowlers =
                [
                    new() { PersonId = 1, SubId = subId, Status = Enums.Status.UsingSub, HangCount = hangCount }
                ]
            }
        };
        var bowlers = new List<BowlerListItemViewModel> { bowler };

        // Act
        bowlers.SetBowlerHangSumByWeeks(weeks);

        // Assert
        Assert.Equal(hangCount, bowler.HangCount);
    }
}