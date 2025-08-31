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
        A.CallTo(() => bowlerRepo.AddBowler(A<Bowler>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.AddBowler(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.AddBowler(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var bowler = new Bowler { Id = 2 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddBowler(A<Bowler>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.AddBowler(bowler);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.AddBowler(A<Bowler>._)).MustHaveHappenedOnceExactly();
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
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekId(A<int>._)).Returns(Task.FromResult<IEnumerable<Bowler>>(expected));

        // Act
        var result = await service.GetAllBowlersByWeekId(weeekId);

        // Assert
        Assert.Equal(expected.Count, result.Count());
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekId(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlers_RepositoryReturnsEmpty_ReturnsEmpty()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllBowlers()).Returns(Task.FromResult<IEnumerable<Bowler>>([]));

        // Act
        var result = await service.GetAllBowlers();

        // Assert
        Assert.Empty(result);
        A.CallTo(() => bowlerRepo.GetAllBowlers()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlerById_ValidId_ReturnsBowler()
    {
        // Arrange
        var id = 10;
        var expected = new Bowler { Id = id };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetBowlerById(A<int>._)).Returns(Task.FromResult(expected));

        // Act
        var result = await service.GetBowlerById(id);

        // Assert
        Assert.Equal(expected.Id, result.Id);
        A.CallTo(() => bowlerRepo.GetBowlerById(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var id = 7;
        var expected = new List<Bowler> { new() { Id = 3, WeekId = id } };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekId(A<int>._)).Returns(Task.FromResult<IEnumerable<Bowler>>(expected));

        // Act
        var result = await service.GetAllBowlersByWeekId(id);

        // Assert
        Assert.Single(result);
        Assert.Equal(expected[0].Id, result.First().Id);
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekId(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_ValidId_ReturnsTrue()
    {
        // Arrange
        var id = 1;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveBowler(id)).Returns(Task.FromResult(true));

        // Act
        var result = await service.RemoveBowler(id);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.RemoveBowler(id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_InvalidId_ReturnsFalse()
    {
        // Arrange
        var id = 999;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveBowler(id)).Returns(Task.FromResult(false));

        // Act
        var result = await service.RemoveBowler(id);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.RemoveBowler(id)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task RemoveBowler_RepositoryThrowsException_PropagatesException()
    {
        // Arrange
        var id = 2;
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.RemoveBowler(id)).Throws(new InvalidOperationException("Repository error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.RemoveBowler(id));
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var expected = new Bowler { Id = 4 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowler(A<Bowler>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.UpdateBowler(expected);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.UpdateBowler(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Bowler { Id = 5 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowler(A<Bowler>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.UpdateBowler(expected);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.UpdateBowler(A<Bowler>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddBowler_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.AddBowler(A<Bowler>._)).Throws(new ArgumentNullException(nameof(Bowler)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddBowler(null!));
    }

    [Fact]
    public async Task UpdateBowler_NullBowler_ThrowsArgumentNullException()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowler(A<Bowler>._)).Throws(new ArgumentNullException(nameof(Bowler)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateBowler(null!));
    }
}