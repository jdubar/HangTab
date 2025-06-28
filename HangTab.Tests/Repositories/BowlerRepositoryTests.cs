using HangTab.Data;
using HangTab.Models;
using HangTab.Repositories.Impl;

namespace HangTab.Tests.Repositories;

public class BowlerRepositoryTests
{
    private readonly IDatabaseContext _context = A.Fake<IDatabaseContext>();

    private BowlerRepository CreateRepo() => new(_context);

    [Fact]
    public async Task AddBowler_CallsContextAndReturnsResult()
    {
        // Arrange
        var repo = CreateRepo();
        var bowler = new Bowler { Id = 1, WeekId = 2, PersonId = 3 };
        A.CallTo(() => _context.AddItemAsync(bowler)).Returns(true);

        // Act
        var result = await repo.AddBowler(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => _context.AddItemAsync(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlersByWeekId_CallsContextWithCorrectPredicate()
    {
        // Arrange
        var repo = CreateRepo();
        var weekId = 5;
        var expected = new List<Bowler> { new() { Id = 1, WeekId = weekId, PersonId = 2 } };
        A.CallTo(() => _context.GetAllWithChildrenAsync(A<System.Linq.Expressions.Expression<Func<Bowler, bool>>>._))
            .Returns(expected);

        // Act
        var result = await repo.GetAllBowlersByWeekId(weekId);

        // Assert
        Assert.Equal(expected, result);
        A.CallTo(() => _context.GetAllWithChildrenAsync(A<System.Linq.Expressions.Expression<Func<Bowler, bool>>>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlers_CallsContext()
    {
        // Arrange
        var repo = CreateRepo();
        var expected = new List<Bowler> { new() { Id = 1 } };
        A.CallTo(() => _context.GetAllAsync<Bowler>()).Returns(expected);

        // Act
        var result = await repo.GetAllBowlers();

        // Assert
        Assert.Equal(expected, result);
        A.CallTo(() => _context.GetAllAsync<Bowler>()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlerById_CallsContextWithId()
    {
        // Arrange
        var id = 7;
        var repo = CreateRepo();
        var bowler = new Bowler { Id = id };
        A.CallTo(() => _context.GetItemByIdAsync<Bowler>(A<int>._)).Returns(bowler);

        // Act
        var result = await repo.GetBowlerById(id);

        // Assert
        Assert.Equal(bowler, result);
        A.CallTo(() => _context.GetItemByIdAsync<Bowler>(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlersByWeekId_CallsContextWithCorrectPredicate()
    {
        // Arrange
        var repo = CreateRepo();
        var weekId = 3;
        var expected = new List<Bowler> { new() { Id = 2, WeekId = weekId } };
        A.CallTo(() => _context.GetFilteredAsync(A<System.Linq.Expressions.Expression<Func<Bowler, bool>>>._))
            .Returns(expected);

        // Act
        var result = await repo.GetBowlersByWeekId(weekId);

        // Assert
        Assert.Equal(expected, result);
        A.CallTo(() => _context.GetFilteredAsync(A<System.Linq.Expressions.Expression<Func<Bowler, bool>>>._))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_CallsContextAndReturnsResult()
    {
        // Arrange
        var repo = CreateRepo();
        var bowler = new Bowler { Id = 4 };
        A.CallTo(() => _context.UpdateItemAsync(bowler)).Returns(true);

        // Act
        var result = await repo.UpdateBowler(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => _context.UpdateItemAsync(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_ThrowsOnNull()
    {
        // Arrange
        var repo = CreateRepo();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddBowler(null!));
    }

    [Fact]
    public async Task GetAllBowlersByWeekId_ThrowsOnInvalidId()
    {
        // Arrange
        var repo = CreateRepo();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetAllBowlersByWeekId(0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetAllBowlersByWeekId(-1));
    }

    [Fact]
    public async Task GetBowlerById_ThrowsOnInvalidId()
    {
        // Arrange
        var repo = CreateRepo();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetBowlerById(0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetBowlerById(-5));
    }

    [Fact]
    public async Task GetBowlersByWeekId_ThrowsOnInvalidId()
    {
        // Arrange
        var repo = CreateRepo();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetBowlersByWeekId(0));
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => repo.GetBowlersByWeekId(-2));
    }

    [Fact]
    public async Task UpdateBowler_ThrowsOnNull()
    {
        // Arrange
        var repo = CreateRepo();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateBowler(null!));
    }
}