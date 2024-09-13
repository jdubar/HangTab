using HangTab.Models;

namespace HangTab.Tests.DatabaseServiceTests;
public class DirectDatabaseTests : TestBase
{
    [Fact]
    public async Task ItShouldDeleteAllTables()
    {
        // Given
        A.CallTo(() => ContextFake.DropTableAsync<Bowler>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BowlerWeek>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BusRide>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BusRideWeek>()).Returns(true);

        // When
        var actual = await DatabaseService.DropAllTables();

        // Then
        actual.Should().BeTrue();
    }
}
