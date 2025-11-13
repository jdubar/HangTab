using HangTab.Enums;
using HangTab.Mappers;
using HangTab.ViewModels.Items;

namespace HangTab.Tests.Mappers;

public class BowlerMapperTests
{
    [Fact]
    public void Map_ValidCurrentWeekListItemViewModel_MapsToBowler()
    {
        // Arrange
        var vm = new CurrentWeekListItemViewModel
        {
            WeekId = 2,
            BowlerId = 10,
            PersonId = 5,
            SubId = 7,
            Status = Status.UsingSub,
            HangCount = 3,
            Name = "Test Bowler",
            IsSub = false,
            ImageUrl = "img.png"
        };

        // Act
        var actual = vm.ToBowler();

        // Assert
        Assert.Equal(10, actual.Id);
        Assert.Equal(5, actual.PersonId);
        Assert.Equal(Status.UsingSub, actual.Status);
        Assert.Equal(3, actual.HangCount);
    }

    [Fact]
    public void Map_NullViewModel_ThrowsArgumentNullException()
    {
        // Arrange
        CurrentWeekListItemViewModel? vm = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => vm.ToBowler());
    }
}