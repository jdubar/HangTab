using HangTab.Mappers;
using HangTab.ViewModels.Items;

namespace HangTab.Tests.Mappers;

public class PersonMapperTests
{
    [Fact]
    public void Map_ValidBowlerListItemViewModel_MapsToPerson()
    {
        // Arrange
        var vm = new BowlerListItemViewModel
        {
            Id = 42,
            Name = "Jane Doe",
            IsSub = true,
            ImageUrl = "img.png"
        };

        // Act
        var person = vm.ToPerson();

        // Assert
        Assert.Equal(42, person.Id);
        Assert.Equal("Jane Doe", person.Name);
        Assert.True(person.IsSub);
        Assert.Equal("img.png", person.ImageUrl);
    }

    [Fact]
    public void Map_NullViewModel_ThrowsArgumentNullException()
    {
        // Arrange
        BowlerListItemViewModel vm = null!;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => vm.ToPerson());
    }
}