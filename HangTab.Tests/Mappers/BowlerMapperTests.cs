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
        var mapper = new BowlerMapper();
        var vm = new CurrentWeekListItemViewModel(
            weekId: 2,
            bowlerId: 10,
            personId: 5,
            subId: 7,
            status: Status.UsingSub,
            hangCount: 3,
            name: "Test Bowler",
            isSub: false,
            initials: "TB",
            imageUrl: "img.png"
        );

        // Act
        var actual = mapper.Map(vm);

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
        var mapper = new BowlerMapper();
        CurrentWeekListItemViewModel? vm = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => mapper.Map(vm!));
    }
}