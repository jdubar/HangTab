using HangTab.Enums;
using HangTab.Mappers;
using HangTab.Models;

namespace HangTab.Tests.Mappers;

public class CurrentWeekListItemViewModelMapperTests
{
    [Fact]
    public void Map_ValidBowlerList_MapsToViewModels()
    {
        // Arrange
        var bowlers = new List<Bowler>
        {
            new() {
                Id = 1,
                WeekId = 2,
                PersonId = 3,
                SubId = 4,
                Status = Status.Active,
                HangCount = 5,
                Person = new Person
                {
                    Id = 3,
                    Name = "Alice Smith",
                    IsSub = false,
                    ImageUrl = "img1.png"
                }
            },
            new() {
                Id = 10,
                WeekId = 20,
                PersonId = 30,
                SubId = null,
                Status = Status.UsingSub,
                HangCount = 7,
                Person = new Person
                {
                    Id = 30,
                    Name = "Bob Jones",
                    IsSub = true,
                    ImageUrl = "img2.png"
                }
            }
        };

        // Act
        var actual = bowlers.ToCurrentWeekListItemViewModelList().ToList();

        // Assert
        Assert.Equal(2, actual.Count);

        Assert.Equal(2, actual[0].WeekId);
        Assert.Equal(1, actual[0].BowlerId);
        Assert.Equal(3, actual[0].PersonId);
        Assert.Equal(4, actual[0].SubId);
        Assert.Equal(Status.Active, actual[0].Status);
        Assert.Equal(5, actual[0].HangCount);
        Assert.Equal("Alice Smith", actual[0].Name);
        Assert.False(actual[0].IsSub);
        Assert.Equal("AS", actual[0].Initials);
        Assert.Equal("img1.png", actual[0].ImageUrl);

        Assert.Equal(20, actual[1].WeekId);
        Assert.Equal(10, actual[1].BowlerId);
        Assert.Equal(30, actual[1].PersonId);
        Assert.Null(actual[1].SubId);
        Assert.Equal(Status.UsingSub, actual[1].Status);
        Assert.Equal(7, actual[1].HangCount);
        Assert.Equal("Bob Jones", actual[1].Name);
        Assert.True(actual[1].IsSub);
        Assert.Equal("BJ", actual[1].Initials);
        Assert.Equal("img2.png", actual[1].ImageUrl);
    }

    [Fact]
    public void Map_NullBowlerList_ThrowsArgumentNullException()
    {
        // Arrange
        List<Bowler> bowlers = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => bowlers.ToCurrentWeekListItemViewModelList());
    }
}