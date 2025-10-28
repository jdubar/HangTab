using HangTab.Mappers;
using HangTab.Models;

namespace HangTab.Tests.Mappers;

public class SubListItemViewModelMapperTests
{
    [Fact]
    public void Map_ValidPersonList_MapsToViewModels()
    {
        // Arrange
        var people = new List<Person>
        {
            new() { Id = 1, Name = "Alice", IsSub = true, ImageUrl = "img1.png" },
            new() { Id = 2, Name = "Bob", IsSub = false, ImageUrl = "img2.png" }
        };

        // Act
        var actual = people.ToSubListItemViewModelList().ToList();

        // Assert
        Assert.Equal(2, actual.Count);

        Assert.Equal(1, actual[0].Id);
        Assert.Equal("Alice", actual[0].Name);
        Assert.True(actual[0].IsSub);
        Assert.Equal("img1.png", actual[0].ImageUrl);

        Assert.Equal(2, actual[1].Id);
        Assert.Equal("Bob", actual[1].Name);
        Assert.False(actual[1].IsSub);
        Assert.Equal("img2.png", actual[1].ImageUrl);
    }

    [Fact]
    public void Map_NullPeopleList_ThrowsArgumentNullException()
    {
        // Arrange
        List<Person> people = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => people.ToSubListItemViewModelList());
    }
}