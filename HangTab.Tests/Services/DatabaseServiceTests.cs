using HangTab.Repositories;
using HangTab.Services.Impl;

namespace HangTab.Tests.Services;

public class DatabaseServiceTests
{
    [Fact]
    public async Task DeleteAllData_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteAllData()).Returns(Task.FromResult(true));

        // Act
        var result = await service.DeleteAllData();

        // Assert
        Assert.True(result);
        A.CallTo(() => databaseRepo.DeleteAllData()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllData_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteAllData()).Returns(Task.FromResult(false));

        // Act
        var result = await service.DeleteAllData();

        // Assert
        Assert.False(result);
        A.CallTo(() => databaseRepo.DeleteAllData()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllData_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteAllData()).Throws(new InvalidOperationException("fail"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(service.DeleteAllData);
    }

    [Fact]
    public async Task DeleteSeasonData_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteSeasonData()).Returns(Task.FromResult(true));

        // Act
        var result = await service.DeleteSeasonData();

        // Assert
        Assert.True(result);
        A.CallTo(() => databaseRepo.DeleteSeasonData()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteSeasonData_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteSeasonData()).Returns(Task.FromResult(false));

        // Act
        var result = await service.DeleteSeasonData();

        // Assert
        Assert.False(result);
        A.CallTo(() => databaseRepo.DeleteSeasonData()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteSeasonData_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteSeasonData()).Throws(new Exception("fail"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(service.DeleteSeasonData);
    }
}