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
        A.CallTo(() => bowlerRepo.AddBowlerAsync(A<Bowler>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.AddBowlerAsync(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.AddBowlerAsync(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var bowler = new Bowler { Id = 2 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddBowlerAsync(A<Bowler>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.AddBowlerAsync(bowler);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.AddBowlerAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekIdAsync(A<int>._)).Returns(Task.FromResult<IEnumerable<Bowler>>(expected));

        // Act
        var result = await service.GetAllBowlersByWeekIdAsync(weeekId);

        // Assert
        Assert.Equal(expected.Count, result.Count());
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlers_RepositoryReturnsEmpty_ReturnsEmpty()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllBowlersAsync()).Returns(Task.FromResult<IEnumerable<Bowler>>([]));

        // Act
        var result = await service.GetAllBowlersAsync();

        // Assert
        Assert.Empty(result);
        A.CallTo(() => bowlerRepo.GetAllBowlersAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlerById_ValidId_ReturnsBowler()
    {
        // Arrange
        var id = 10;
        var expected = new Bowler { Id = id };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetBowlerByIdAsync(A<int>._)).Returns(Task.FromResult(expected));

        // Act
        var result = await service.GetBowlerByIdAsync(id);

        // Assert
        Assert.Equal(expected.Id, result.Id);
        A.CallTo(() => bowlerRepo.GetBowlerByIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var id = 7;
        var expected = new List<Bowler> { new() { Id = 3, WeekId = id } };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekIdAsync(A<int>._)).Returns(Task.FromResult<IEnumerable<Bowler>>(expected));

        // Act
        var result = await service.GetAllBowlersByWeekIdAsync(id);

        // Assert
        Assert.Single(result);
        Assert.Equal(expected[0].Id, result.First().Id);
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekIdAsync(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_ValidId_ReturnsTrue()
    {
        // Arrange
        var id = 1;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveBowlerAsync(id)).Returns(Task.FromResult(true));

        // Act
        var result = await service.RemoveBowlerAsync(id);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.RemoveBowlerAsync(id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_InvalidId_ReturnsFalse()
    {
        // Arrange
        var id = 999;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveBowlerAsync(id)).Returns(Task.FromResult(false));

        // Act
        var result = await service.RemoveBowlerAsync(id);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.RemoveBowlerAsync(id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var id = 2;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveBowlerAsync(id)).Throws(new InvalidOperationException("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RemoveBowlerAsync(id));
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var expected = new Bowler { Id = 4 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowlerAsync(A<Bowler>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.UpdateBowlerAsync(expected);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.UpdateBowlerAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Bowler { Id = 5 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowlerAsync(A<Bowler>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.UpdateBowlerAsync(expected);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.UpdateBowlerAsync(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddBowlerAsync(A<Bowler>._)).Throws(new ArgumentNullException(nameof(Bowler)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddBowlerAsync(null!));
    }

    [Fact]
    public async Task UpdateBowler_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowlerAsync(A<Bowler>._)).Throws(new ArgumentNullException(nameof(Bowler)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateBowlerAsync(null!));
    }
}