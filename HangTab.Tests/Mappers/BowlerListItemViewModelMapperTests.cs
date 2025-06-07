using HangTab.Enums;
using HangTab.Mappers;
using HangTab.Models;

namespace HangTab.Tests.Mappers;

public class BowlerListItemViewModelMapperTests
{
    [Fact]
    public void Map_FromPersonList_ReturnsCorrectViewModel()
    {
        // Arrange
        var mapper = new BowlerListItemViewModelMapper();
        var people = new List<Person>
        {
            new() { Id = 1, Name = "Alice", IsSub = false, ImageUrl = "img1" },
            new() { Id = 2, Name = "Bob", IsSub = true, ImageUrl = "img2" }
        };

        // Act
        var actual = mapper.Map(people).ToList();

        // Assert
        Assert.Equal(2, actual.Count);

        Assert.Equal(1, actual[0].Id);
        Assert.Equal("Alice", actual[0].Name);
        Assert.False(actual[0].IsSub);
        Assert.Equal("img1", actual[0].ImageUrl);

        Assert.Equal(2, actual[1].Id);
        Assert.Equal("Bob", actual[1].Name);
        Assert.True(actual[1].IsSub);
        Assert.Equal("img2", actual[1].ImageUrl);
    }

    [Fact]
    public void Map_FromBowlerList_ReturnsCorrectViewModel()
    {
        // Arrange
        var mapper = new BowlerListItemViewModelMapper();
        var bowlers = new List<Bowler>
        {
            new() {
                Id = 10,
                PersonId = 1,
                HangCount = 5,
                Status = Status.UsingSub,
                Person = new Person { Id = 1, Name = "Charlie", IsSub = true, ImageUrl = "img3" }
            }
        };

        // Act
        var vm = mapper.Map(bowlers).ToList();

        // Assert
        Assert.Single(vm);
        var actual = vm[0];
        Assert.Equal(1, actual.Id);
        Assert.Equal("Charlie", actual.Name);
        Assert.True(actual.IsSub);
        Assert.Equal(5, actual.Hangings);
        Assert.Equal(10, actual.BowlerId);
        Assert.Equal("img3", actual.ImageUrl);
        Assert.Equal(Status.UsingSub, actual.Status);
    }

    [Fact]
    public void Map_NullPerson_ThrowsArgumentNullException()
    {
        // Arrange
        var mapper = new BowlerListItemViewModelMapper();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mapper.Map((IEnumerable<Person>)null!));
    }

    [Fact]
    public void Map_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var mapper = new BowlerListItemViewModelMapper();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mapper.Map((IEnumerable<Bowler>)null!));
    }
}