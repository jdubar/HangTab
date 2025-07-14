using HangTab.Data;
using HangTab.Models;
using HangTab.Repositories.Impl;

using System.Linq.Expressions;

namespace HangTab.Tests.Repositories;

public class BowlerRepositoryTests
{
    [Fact]
    public async Task AddBowler_CallsContextAndReturnsResult()
    {
        // Arrange
        var bowler = new Bowler { Id = 1, WeekId = 2, PersonId = 3 };
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);
        A.CallTo(() => context.AddItemAsync(A<Bowler>._)).Returns(true);

        // Act
        var actual = await bowlerRepository.AddBowler(bowler);

        // Assert
        Assert.True(actual);
        A.CallTo(() => context.AddItemAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlersByWeekId_CallsContextWithCorrectPredicate()
    {
        // Arrange
        var weekId = 5;
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);
        var expected = new List<Bowler> { new() { Id = 1, WeekId = weekId, PersonId = 2 } };
        A.CallTo(() => context.GetAllWithChildrenAsync(A<Expression<Func<Bowler, bool>>>._))
            .Returns(expected);

        // Act
        var actual = await bowlerRepository.GetAllBowlersByWeekId(weekId);

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetAllWithChildrenAsync(A<Expression<Func<Bowler, bool>>>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlers_CallsContext()
    {
        // Arrange
        var expected = new List<Bowler> { new() { Id = 1 } };
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);
        A.CallTo(() => context.GetAllWithChildrenAsync<Bowler>(null)).Returns(expected);

        // Act
        var actual = await bowlerRepository.GetAllBowlers();

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetAllWithChildrenAsync<Bowler>(null)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlerById_CallsContextWithId()
    {
        // Arrange
        var id = 7;
        var bowler = new Bowler { Id = id };
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);
        A.CallTo(() => context.GetItemByIdAsync<Bowler>(A<int>._)).Returns(bowler);

        // Act
        var result = await bowlerRepository.GetBowlerById(id);

        // Assert
        Assert.Equal(bowler, result);
        A.CallTo(() => context.GetItemByIdAsync<Bowler>(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlersByWeekId_CallsContextWithCorrectPredicate()
    {
        // Arrange
        var weekId = 3;
        var expected = new List<Bowler> { new() { Id = 2, WeekId = weekId } };
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);
        A.CallTo(() => context.GetAllWithChildrenAsync(A<Expression<Func<Bowler, bool>>>._))
            .Returns(expected);

        // Act
        var actual = await bowlerRepository.GetAllBowlersByWeekId(weekId);

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetAllWithChildrenAsync(A<Expression<Func<Bowler, bool>>>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_CallsContextAndReturnsResult()
    {
        // Arrange
        var bowler = new Bowler { Id = 4 };
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);
        A.CallTo(() => context.UpdateItemAsync(A<Bowler>._)).Returns(true);

        // Act
        var result = await bowlerRepository.UpdateBowler(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => context.UpdateItemAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_ThrowsOnNull()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => bowlerRepository.AddBowler(null!));
    }

    [Fact]
    public async Task GetAllBowlersByWeekId_ThrowsOnInvalidId()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bowlerRepository.GetAllBowlersByWeekId(0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bowlerRepository.GetAllBowlersByWeekId(-1));
    }

    [Fact]
    public async Task GetBowlerById_ThrowsOnInvalidId()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bowlerRepository.GetBowlerById(0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bowlerRepository.GetBowlerById(-5));
    }

    [Fact]
    public async Task GetBowlersByWeekId_ThrowsOnInvalidId()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bowlerRepository.GetAllBowlersByWeekId(0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => bowlerRepository.GetAllBowlersByWeekId(-2));
    }

    [Fact]
    public async Task UpdateBowler_ThrowsOnNull()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var bowlerRepository = new BowlerRepository(context);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => bowlerRepository.UpdateBowler(null!));
    }
}