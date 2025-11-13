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
        var weekRepo = A.Fake<IBaseRepository<Week>>();
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new WeekService(weekRepo, personRepo, bowlerRepo);
        A.CallTo(() => weekRepo.GetByIdAsync(A<int>._)).Returns(expected);

        // Act
        var result = await service.GetByIdAsync(1);

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Number, actual.Number);
    }

    [Fact]
    public async Task GetAllWeeks_RepositoryReturnsWeeks_ReturnsWeeks()
    {
        // Arrange
        var expected = new List<Week> { new() { Id = 1 }, new() { Id = 2 } };
        var weekRepo = A.Fake<IBaseRepository<Week>>();
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new WeekService(weekRepo, personRepo, bowlerRepo);
        A.CallTo(() => weekRepo.GetAllAsync()).Returns(expected);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Count, actual.Count());
    }

    [Fact]
    public async Task CreateWeek_ValidNumber_ReturnsWeek()
    {
        // Arrange
        var expected = new Week { Id = 3, Number = 4 };
        var weekRepo = A.Fake<IBaseRepository<Week>>();
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new WeekService(weekRepo, personRepo, bowlerRepo);
        A.CallTo(() => weekRepo.AddAsync(A<Week>._)).Returns(true);

        // Act
        var result = await service.AddAsync(4);

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Number, actual.Number);
    }

    [Fact]
    public async Task UpdateWeek_ValidWeek_CallsRepository()
    {
        // Arrange
        var week = new Week { Id = 5, Number = 6 };
        var weekRepo = A.Fake<IBaseRepository<Week>>();
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var bowlerRepo = A.Fake<IBaseRepository<Bowler>>();
        var service = new WeekService(weekRepo, personRepo, bowlerRepo);
        A.CallTo(() => weekRepo.UpdateAsync(A<Week>._)).Returns(true);

        // Act
        var result = await service.UpdateAsync(week);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
