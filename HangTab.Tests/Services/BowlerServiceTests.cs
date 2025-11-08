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
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddAsync(A<Bowler>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.AddAsync(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.AddAsync(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var bowler = new Bowler { Id = 2 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddAsync(A<Bowler>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.AddAsync(bowler);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.AddAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
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

        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllByWeekIdAsync(A<int>._)).Returns(Task.FromResult<IEnumerable<Bowler>>(expected));

        // Act
        var result = await service.GetAllByWeekIdAsync(weeekId);

        // Assert
        Assert.Equal(expected.Count, result.Count());
        A.CallTo(() => bowlerRepo.GetAllByWeekIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlers_RepositoryReturnsEmpty_ReturnsEmpty()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllAsync()).Returns(Task.FromResult<IEnumerable<Bowler>>([]));

        // Act
        var result = await service.GetAllAsync();

        // Assert
        Assert.Empty(result);
        A.CallTo(() => bowlerRepo.GetAllAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlerById_ValidId_ReturnsBowler()
    {
        // Arrange
        var id = 10;
        var expected = new Bowler { Id = id };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetByIdAsync(A<int>._)).Returns(Task.FromResult(expected));

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        Assert.Equal(expected.Id, result.Id);
        A.CallTo(() => bowlerRepo.GetByIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var id = 7;
        var expected = new List<Bowler> { new() { Id = 3, WeekId = id } };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllByWeekIdAsync(A<int>._)).Returns(Task.FromResult<IEnumerable<Bowler>>(expected));

        // Act
        var result = await service.GetAllByWeekIdAsync(id);

        // Assert
        Assert.Single(result);
        Assert.Equal(expected[0].Id, result.First().Id);
        A.CallTo(() => bowlerRepo.GetAllByWeekIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_ValidId_ReturnsTrue()
    {
        // Arrange
        var id = 1;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveAsync(id)).Returns(Task.FromResult(true));

        // Act
        var result = await service.RemoveAsync(id);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.RemoveAsync(id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_InvalidId_ReturnsFalse()
    {
        // Arrange
        var id = 999;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveAsync(id)).Returns(Task.FromResult(false));

        // Act
        var result = await service.RemoveAsync(id);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.RemoveAsync(id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var id = 2;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveAsync(id)).Throws(new InvalidOperationException("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RemoveAsync(id));
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var expected = new Bowler { Id = 4 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.UpdateAsync(expected);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Bowler { Id = 5 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.UpdateAsync(expected);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddAsync(A<Bowler>._)).Throws(new ArgumentNullException(nameof(Bowler)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
    }

    [Fact]
    public async Task UpdateBowler_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateAsync(A<Bowler>._)).Throws(new ArgumentNullException(nameof(Bowler)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
    }
}