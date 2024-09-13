using HangTab.Models;
using HangTab.Tests.TestData;

namespace HangTab.Tests.DatabaseServiceTests;
public class DirectDatabaseTests : TestBase
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ItShouldAttemptToDeleteAllTables(bool expected)
    {
        // Given
        A.CallTo(() => ContextFake.DropTableAsync<Bowler>()).Returns(expected);
        A.CallTo(() => ContextFake.DropTableAsync<BowlerWeek>()).Returns(expected);
        A.CallTo(() => ContextFake.DropTableAsync<BusRide>()).Returns(expected);
        A.CallTo(() => ContextFake.DropTableAsync<BusRideWeek>()).Returns(expected);

        // When
        var actual = await DatabaseService.DropAllTables();

        // Then
        actual.Should().Be(expected);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task ItShouldAttemptToResetAllBowlerHangings(bool expected)
    {
        // Given
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(SimpleData.ListOfFiveBowlers);
        A.CallTo(() => ContextFake.UpdateItemAsync(A<Bowler>.Ignored)).Returns(expected);
        A.CallTo(() => ContextFake.DropTableAsync<BowlerWeek>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BusRide>()).Returns(true);
        A.CallTo(() => ContextFake.DropTableAsync<BusRideWeek>()).Returns(true);

        // When
        var actual = await DatabaseService.ResetHangings();

        // Then
        actual.Should().Be(expected);
    }

    [Fact]
    public async Task ItShouldThrowOnResetAllBowlerHangings()
    {
        // Given
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(SimpleData.ListOfFiveBowlers);
        A.CallTo(() => ContextFake.UpdateItemAsync(A<Bowler>.Ignored)).Throws<Exception>().Once().Then.Returns(false);
        //A.CallTo(() => ContextFake.DropTableAsync<BowlerWeek>()).Returns(true);
        //A.CallTo(() => ContextFake.DropTableAsync<BusRide>()).Returns(true);
        //A.CallTo(() => ContextFake.DropTableAsync<BusRideWeek>()).Returns(true);

        // When
        var actual = await DatabaseService.ResetHangings();

        // Then
        actual.Should().BeFalse();
    }
}
