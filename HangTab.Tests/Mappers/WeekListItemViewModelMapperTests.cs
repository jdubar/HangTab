using HangTab.Enums;
using HangTab.Mappers;
using HangTab.Models;

namespace HangTab.Tests.Mappers;

public class WeekListItemViewModelMapperTests
{
    [Fact]
    public void Map_ValidWeeks_MapsToViewModels()
    {
        // Arrange
        var weeks = new List<Week>
        {
            new()
            {
                Id = 1,
                Number = 2,
                BusRides = 3,
                Bowlers =
                [
                    new()
                    {
                        Id = 10,
                        PersonId = 100,
                        HangCount = 5,
                        Status = Status.Active,
                        Person = new Person { Id = 100, Name = "Alice", IsSub = false, ImageUrl = "img1" }
                    },
                    new()
                    {
                        Id = 11,
                        PersonId = 101,
                        HangCount = 2,
                        Status = Status.UsingSub,
                        Person = new Person { Id = 101, Name = "Bob", IsSub = true, ImageUrl = "img2" }
                    }
                ]
            }
        };

        // Act
        var actual = weeks.ToWeekListItemViewModelList().ToList();

        // Assert
        Assert.Single(actual);
        var vm = actual[0];
        Assert.Equal(1, vm.Id);
        Assert.Equal(2, vm.Number);
        Assert.Equal(3, vm.BusRides);
        Assert.Equal(7, vm.TotalHangCount); // 5 + 2
    }

    [Fact]
    public void Map_EmptyWeeks_ReturnsEmpty()
    {
        // Arrange
        var weeks = new List<Week>();

        // Act
        var actual = weeks.ToWeekListItemViewModelList().ToList();

        // Assert
        Assert.Empty(actual);
    }

    [Fact]
    public void Map_NullWeeks_ThrowsArgumentNullException()
    {
        // Arrange
        List<Week>? weeks = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => weeks.ToWeekListItemViewModelList());
    }

    [Fact]
    public void Map_WeekWithNullBowlers_ThrowsArgumentNullException()
    {
        // Arrange
        var weeks = new List<Week>
        {
            new()
            {
                Id = 1,
                Number = 2,
                BusRides = 3,
                Bowlers = null!
            }
        };

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => weeks.ToWeekListItemViewModelList().ToList());
    }
}