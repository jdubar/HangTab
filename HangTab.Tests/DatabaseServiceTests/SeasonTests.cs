using HangTab.Models;

namespace HangTab.Tests.DatabaseServiceTests;
public class SeasonTests : TestBase
{
    [Fact]
    public async Task ItShouldGetSeasonSettings()
    {
        // Given
        A.CallTo(() => ContextFake.GetAllAsync<SeasonSettings>()).Returns(new List<SeasonSettings>());

        // When
        var actual = await DatabaseService.GetSeasonSettings();

        // Then
        actual.Should().BeEquivalentTo(new SeasonSettings());
    }

    [Fact]
    public async Task ItShouldAddSeasonSettings()
    {
        // Given
        var vm = new SeasonSettings() { CostPerHang = 0.25M, TotalSeasonWeeks = 34 };
        A.CallTo(() => ContextFake.GetAllAsync<SeasonSettings>()).Returns(new List<SeasonSettings>());
        A.CallTo(() => ContextFake.AddItemAsync(A<SeasonSettings>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.UpdateSeasonSettings(vm);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldUpdateSeasonSettings()
    {
        // Given
        var vm = new SeasonSettings() { CostPerHang = 0.25M, TotalSeasonWeeks = 34 };
        var expected = new List<SeasonSettings>
        {
            new(){ CostPerHang = 0.25M, TotalSeasonWeeks = 34 }
        };
        A.CallTo(() => ContextFake.GetAllAsync<SeasonSettings>()).Returns(expected);
        A.CallTo(() => ContextFake.UpdateItemAsync(A<SeasonSettings>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.UpdateSeasonSettings(vm);

        // Then
        actual.Should().BeTrue();
    }
}
