using HangTab.Enums;
using HangTab.Mappers;
using HangTab.ViewModels.Items;

namespace HangTab.Tests.Mappers;

public class PersonMapperTests
{
    [Fact]
    public void Map_ValidBowlerListItemViewModel_MapsToPerson()
    {
        // Arrange
        var mapper = new PersonMapper();
        var vm = new BowlerListItemViewModel(
            id: 42,
            name: "Jane Doe",
            isSub: true,
            bowlerId: 7,
            hangings: 3,
            imageUrl: "img.png",
            status: Status.UsingSub
        );

        // Act
        var person = mapper.Map(vm);

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
        var mapper = new PersonMapper();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mapper.Map(null!));
    }
}