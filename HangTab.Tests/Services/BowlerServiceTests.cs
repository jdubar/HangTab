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
        A.CallTo(() => bowlerRepo.AddBowler(bowler)).Returns(Task.FromResult(true));

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
        A.CallTo(() => bowlerRepo.AddBowler(bowler)).Returns(Task.FromResult(false));

        // Act
        var result = await service.AddBowler(bowler);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.AddBowler(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var bowlers = new List<Bowler> { new() { Id = 1 }, new() { Id = 2 } };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekId(5)).Returns(Task.FromResult<IEnumerable<Bowler>>(bowlers));

        // Act
        var result = await service.GetAllBowlersByWeekId(5);

        // Assert
        Assert.Equal(2, result.Count());
        A.CallTo(() => bowlerRepo.GetAllBowlersByWeekId(5)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllBowlers_RepositoryReturnsEmpty_ReturnsEmpty()
    {
        // Arrange
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetAllBowlers()).Returns(Task.FromResult<IEnumerable<Bowler>>(new List<Bowler>()));

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
        var bowler = new Bowler { Id = 10 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetBowlerById(10)).Returns(Task.FromResult(bowler));

        // Act
        var result = await service.GetBowlerById(10);

        // Assert
        Assert.Equal(10, result.Id);
        A.CallTo(() => bowlerRepo.GetBowlerById(10)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetBowlersByWeekId_ValidId_ReturnsBowlers()
    {
        // Arrange
        var bowlers = new List<Bowler> { new Bowler { Id = 3 } };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.GetBowlersByWeekId(7)).Returns(Task.FromResult<IEnumerable<Bowler>>(bowlers));

        // Act
        var result = await service.GetBowlersByWeekId(7);

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result.First().Id);
        A.CallTo(() => bowlerRepo.GetBowlersByWeekId(7)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var bowler = new Bowler { Id = 4 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowler(bowler)).Returns(Task.FromResult(true));

        // Act
        var result = await service.UpdateBowler(bowler);

        // Assert
        Assert.True(result);
        A.CallTo(() => bowlerRepo.UpdateBowler(bowler)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateBowler_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var bowler = new Bowler { Id = 5 };
        var bowlerRepo = A.Fake<IBowlerRepository>();
        var service = new BowlerService(bowlerRepo);
        A.CallTo(() => bowlerRepo.UpdateBowler(bowler)).Returns(Task.FromResult(false));

        // Act
        var result = await service.UpdateBowler(bowler);

        // Assert
        Assert.False(result);
        A.CallTo(() => bowlerRepo.UpdateBowler(bowler)).MustHaveHappenedOnceExactly();
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