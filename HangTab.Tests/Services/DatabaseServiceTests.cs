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
        A.CallTo(() => databaseRepo.DeleteAllDataAsync()).Returns(Task.FromResult(true));

        // Act
        var result = await service.DeleteAllDataAsync();

        // Assert
        Assert.True(result);
        A.CallTo(() => databaseRepo.DeleteAllDataAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllData_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteAllDataAsync()).Returns(Task.FromResult(false));

        // Act
        var result = await service.DeleteAllDataAsync();

        // Assert
        Assert.False(result);
        A.CallTo(() => databaseRepo.DeleteAllDataAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllData_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteAllDataAsync()).Throws(new InvalidOperationException("fail"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(service.DeleteAllDataAsync);
    }

    [Fact]
    public async Task DeleteSeasonData_RepositoryReturnsTrue_ReturnsTrue()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteSeasonDataAsync()).Returns(Task.FromResult(true));

        // Act
        var result = await service.DeleteSeasonDataAsync();

        // Assert
        Assert.True(result);
        A.CallTo(() => databaseRepo.DeleteSeasonDataAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteSeasonData_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteSeasonDataAsync()).Returns(Task.FromResult(false));

        // Act
        var result = await service.DeleteSeasonDataAsync();

        // Assert
        Assert.False(result);
        A.CallTo(() => databaseRepo.DeleteSeasonDataAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteSeasonData_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.DeleteSeasonDataAsync()).Throws(new Exception("fail"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(service.DeleteSeasonDataAsync);
    }

    [Fact]
    public async Task InitializeDatabase_CallsRepositoryInitializeDatabase()
    {
        // Arrange
        var databaseRepo = A.Fake<IDatabaseRepository>();
        var service = new DatabaseService(databaseRepo);
        A.CallTo(() => databaseRepo.InitializeDatabaseAsync()).Returns(Task.CompletedTask);

        // Act
        await service.InitializeDatabaseAsync();

        // Assert
        A.CallTo(() => databaseRepo.InitializeDatabaseAsync()).MustHaveHappenedOnceExactly();
    }
}