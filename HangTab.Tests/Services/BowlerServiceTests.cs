using HangTab.Models;
using HangTab.Repositories;
using HangTab.Services.Impl;

namespace HangTab.Tests.Services;

public class BowlerServiceTests
{
    [Fact]
    public async Task AddBowler_ValidBowler_ReturnsTrue()
    {
        // Arrange
        var bowler = new Bowler { Id = 1 };
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddAsync(A<Bowler>._)).Returns(true);

        // Act
        var result = await service.AddAsync(bowler);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AddBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var bowler = new Bowler { Id = 2 };
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddAsync(A<Bowler>._)).Returns(false);

        // Act
        var result = await service.AddAsync(bowler);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task GetAllBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var weeekId = 5;
        var expected = new List<Bowler>
        {
            new() { Id = 1, WeekId = weeekId },
            new() { Id = 2, WeekId = weeekId }
        };

        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetFilteredAsync(A<System.Linq.Expressions.Expression<Func<Bowler, bool>>>._)).Returns(expected);

        // Act
        var result = await service.GetAllByWeekIdAsync(weeekId);

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Count, actual.Count());
    }

    [Fact]
    public async Task GetAllBowlers_RepositoryReturnsEmpty_ReturnsEmpty()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllAsync()).Returns([]);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        var actual = result.Value;
        Assert.Empty(actual);
    }

    [Fact]
    public async Task GetBowlerById_ValidId_ReturnsBowler()
    {
        // Arrange
        var id = 10;
        var expected = new Bowler { Id = id };
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetByIdAsync(A<int>._)).Returns(expected);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Id, actual.Id);
    }

    [Fact]
    public async Task GetBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var id = 7;
        var expected = new List<Bowler> { new() { Id = 3, WeekId = id } };
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetFilteredAsync(A<System.Linq.Expressions.Expression<Func<Bowler, bool>>>._)).Returns(expected);

        // Act
        var result = await service.GetAllByWeekIdAsync(id);

        // Assert
        var actual = result.Value;
        Assert.Single(actual);
        Assert.Equal(expected[0].Id, actual.First().Id);
    }

    [Fact]
    public async Task RemoveBowler_ValidId_ReturnsTrue()
    {
        // Arrange
        var id = 1;
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.DeleteByIdAsync(A<int>._)).Returns(true);

        // Act
        var result = await service.RemoveAsync(id);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task RemoveBowler_InvalidId_ReturnsFalse()
    {
        // Arrange
        var id = 999;
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.DeleteByIdAsync(A<int>._)).Returns(false);

        // Act
        var result = await service.RemoveAsync(id);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task RemoveBowler_NegativeId_ReturnsFalse()
    {
        // Arrange
        var id = -2;
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.DeleteByIdAsync(A<int>._)).Returns(false);

        // Act
        var result = await service.RemoveAsync(id);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var expected = new Bowler { Id = 4 };
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).Returns(true);

        // Act
        var result = await service.UpdateAsync(expected);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Bowler { Id = 5 };
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).Returns(false);

        // Act
        var result = await service.UpdateAsync(expected);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task UpdateBowler_NegativeId_ReturnsFalse()
    {
        // Arrange
        var id = -2;
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).Returns(false);

        // Act
        var result = await service.RemoveAsync(id);

        // Assert
        Assert.True(result.IsFailed);
    }
}