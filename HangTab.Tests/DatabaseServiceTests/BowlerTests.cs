using HangTab.Models;
using HangTab.Tests.TestData;

using System.Linq.Expressions;

namespace HangTab.Tests.DatabaseServiceTests;

public class BowlerTests : TestBase
{
    [Fact]
    public async Task ItShouldAddMainBowlerToTheDatabase()
    {
        // Given
        A.CallTo(() => ContextFake.AddItemAsync(A<Bowler>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.AddBowler(SimpleData.OneBowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldDeleteTheBowlerFromTheDatabase()
    {
        // Given
        A.CallTo(() => ContextFake.DeleteItemByIdAsync<Bowler>(A<int>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.DeleteBowler(SimpleData.OneBowler.Id);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldCheckIfBowlerExists()
    {
        // Given
        var bowlers = new List<Bowler> { SimpleData.OneBowler };
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(bowlers);

        // When
        var actual = await DatabaseService.IsBowlerExists(SimpleData.OneBowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldReturnOnlySubBowlers()
    {
        // Given
        var sub = new List<Bowler> { SimpleData.OneSubBowler };
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(sub);

        // When
        var actual = await DatabaseService.GetFilteredBowlers(b => b.IsSub);

        // Then
        actual.Should().BeEquivalentTo(sub);
    }

    [Fact]
    public async Task ItShouldReturnAllBowlers()
    {
        // Given
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(SimpleData.ListOfFiveBowlers);

        // When
        var actual = await DatabaseService.GetAllBowlers();

        // Then
        actual.Should().BeEquivalentTo(SimpleData.ListOfFiveBowlers);
    }

    [Fact]
    public async Task ItShouldReturnMainBowlersByWeekNumber()
    {
        // Given
        const int week = 1;
        var expected = SimpleData.ListOfMainBowlerViewModels;
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(SimpleData.ListOfMainBowlers);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BowlerWeek, bool>>>.Ignored)).Returns(SimpleData.ListOfTwoBowlerWeeks);

        // When
        var actual = await DatabaseService.GetMainBowlersByWeek(week);

        // Then
        actual.Should().BeEquivalentTo(expected);
    }
}