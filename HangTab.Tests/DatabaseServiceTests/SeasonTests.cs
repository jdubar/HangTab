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
}
