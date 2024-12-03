using HangTab.Models;
using HangTab.Models.ViewModels;
using HangTab.Tests.TestData;

using System.Linq.Expressions;
using HangTab.Services.Impl;

namespace HangTab.Tests.DatabaseServiceTests;

public class BowlerTests : TestBase
{
    [Fact]
    public async Task ItShouldAddMainBowlerToTheDatabase()
    {
        // Given
        var service = new BowlerService(ContextFake);
        A.CallTo(() => ContextFake.AddItemAsync(A<Bowler>.Ignored)).Returns(true);

        // When
        var actual = await service.Add(SimpleData.OneBowler);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldDeleteTheBowlerFromTheDatabase()
    {
        // Given
        var service = new BowlerService(ContextFake);
        A.CallTo(() => ContextFake.DeleteItemByIdAsync<Bowler>(A<int>.Ignored)).Returns(true);

        // When
        var actual = await service.Delete(SimpleData.OneBowler.Id);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldCheckIfBowlerExists()
    {
        // Given
        var service = new BowlerService(ContextFake);
        var bowlers = new List<Bowler> { SimpleData.OneBowler };
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(bowlers);

        // When
        var actual = await service.Exists(SimpleData.OneBowler);

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
        var service = new BowlerService(ContextFake);
        A.CallTo(() => ContextFake.GetAllAsync<Bowler>()).Returns(ListData.ListOfFiveBowlers);

        // When
        var actual = await service.GetAll();

        // Then
        actual.Should().BeEquivalentTo(ListData.ListOfFiveBowlers);
    }

    [Fact]
    public async Task ItShouldReturnMainBowlersByWeekNumber()
    {
        // Given
        const int week = 1;
        var expected = ListData.ListOfMainBowlerViewModels;
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored)).Returns(ListData.ListOfMainBowlers);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BowlerWeek, bool>>>.Ignored)).Returns(ListData.ListOfTwoBowlerWeeks);

        // When
        var actual = await DatabaseService.GetMainBowlersByWeek(week);

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task ItShouldFailUpdatingTheBowlerForHangingsByWeek()
    {
        // Given
        A.CallTo(() => ContextFake.UpdateItemAsync(A<Bowler>.Ignored)).Returns(false);

        // When
        var actual = await DatabaseService.UpdateBowlerHangingsByWeek(new BowlerViewModel(), 1);

        // Then
        actual.Should().BeFalse();
    }

    [Fact]
    public async Task ItShouldAddTheBowlerHangingsByWeek()
    {
        // Given
        A.CallTo(() => ContextFake.UpdateItemAsync(A<Bowler>.Ignored)).Returns(true);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BowlerWeek, bool>>>.Ignored)).Returns(new List<BowlerWeek>());
        A.CallTo(() => ContextFake.AddItemAsync(A<BowlerWeek>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.UpdateBowlerHangingsByWeek(new BowlerViewModel(), 1);

        // Then
        actual.Should().BeTrue();
    }

    [Fact]
    public async Task ItShouldUpdateTheBowlerHangingsByWeek()
    {
        // Given
        A.CallTo(() => ContextFake.UpdateItemAsync(A<Bowler>.Ignored)).Returns(true);
        A.CallTo(() => ContextFake.GetFilteredAsync(A<Expression<Func<BowlerWeek, bool>>>.Ignored)).Returns(ListData.ListOfTwoBowlerWeeks);
        A.CallTo(() => ContextFake.UpdateItemAsync(A<BowlerWeek>.Ignored)).Returns(true);

        // When
        var actual = await DatabaseService.UpdateBowlerHangingsByWeek(new BowlerViewModel(), 1);

        // Then
        actual.Should().BeTrue();
    }
}