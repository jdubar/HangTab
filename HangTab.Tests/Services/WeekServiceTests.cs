using HangTab.Models;
using HangTab.Repositories;
using HangTab.Services.Impl;

namespace HangTab.Tests.Services;

public class WeekServiceTests
{
    [Fact]
    public async Task GetWeekById_ValidId_ReturnsWeek()
    {
        // Arrange
        var expected = new Week { Id = 1, Number = 7 };
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.GetWeekById(A<int>._)).Returns(Task.FromResult(expected));

        // Act
        var result = await service.GetWeekById(1);

        // Assert
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Number, result.Number);
        A.CallTo(() => weekRepo.GetWeekById(1)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllWeeks_RepositoryReturnsWeeks_ReturnsWeeks()
    {
        // Arrange
        var expected = new List<Week> { new() { Id = 1 }, new() { Id = 2 } };
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.GetAllWeeks()).Returns(Task.FromResult<IEnumerable<Week>>(expected));

        // Act
        var result = await service.GetAllWeeks();

        // Assert
        Assert.Equal(expected.Count, result.Count());
        A.CallTo(() => weekRepo.GetAllWeeks()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateWeek_ValidNumber_ReturnsWeek()
    {
        // Arrange
        var expected = new Week { Id = 3, Number = 4 };
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.CreateWeek(A<int>._)).Returns(Task.FromResult(expected));

        // Act
        var result = await service.CreateWeek(4);

        // Assert
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal(expected.Number, result.Number);
        A.CallTo(() => weekRepo.CreateWeek(4)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdateWeek_ValidWeek_CallsRepository()
    {
        // Arrange
        var week = new Week { Id = 5, Number = 6 };
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.UpdateWeek(A<Week>._)).Returns(Task.CompletedTask);

        // Act
        await service.UpdateWeek(week);

        // Assert
        A.CallTo(() => weekRepo.UpdateWeek(week)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetWeekById_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var id = 99;
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.GetWeekById(A<int>._)).Throws(new InvalidOperationException("Not found"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GetWeekById(id));
    }

    [Fact]
    public async Task CreateWeek_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var id = 42;
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.CreateWeek(A<int>._)).Throws(new Exception("Create failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.CreateWeek(id));
    }

    [Fact]
    public async Task UpdateWeek_RepositoryThrows_PropagatesException()
    {
        // Arrange
        var week = new Week { Id = 7, Number = 8 };
        var weekRepo = A.Fake<IWeekRepository>();
        var service = new WeekService(weekRepo);
        A.CallTo(() => weekRepo.UpdateWeek(A<Week>._)).Throws(new Exception("Update failed"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => service.UpdateWeek(week));
    }
}
