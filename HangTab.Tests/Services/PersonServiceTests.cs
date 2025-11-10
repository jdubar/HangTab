using HangTab.Models;
using HangTab.Repositories;
using HangTab.Services.Impl;

namespace HangTab.Tests.Services;

public class PersonServiceTests
{
    [Fact]
    public async Task GetPersonById_ValidId_ReturnsPerson()
    {
        // Arrange
        var id = 42;
        var expected = new Person { Id = id, Name = "Jayden" };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetByIdAsync(A<int>._)).Returns(expected);

        // Act
        var result = await service.GetByIdAsync(id);

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal("Jayden", actual.Name);
    }

    [Fact]
    public async Task GetAllPeople_RepositoryReturnsPeople_ReturnsPeople()
    {
        // Arrange
        var expected = new List<Person> { new() { Id = 16 }, new() { Id = 17 } };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetAllAsync()).Returns(expected);

        // Act
        var result = await service.GetAllAsync();

        // Assert
        var actual = result.Value;
        Assert.Equal(expected.Count, actual.Count());
    }

    [Fact]
    public async Task GetRegulars_RepositoryReturnsRegulars_ReturnsRegulars()
    {
        // Arrange
        var expected = new List<Person> { new() { Id = 42, IsSub = false } };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetFilteredAsync(A<System.Linq.Expressions.Expression<Func<Person, bool>>>._)).Returns(expected);

        // Act
        var result = await service.GetRegularsAsync();

        // Assert
        var actual = result.Value;
        Assert.Single(actual);
        Assert.False(actual.First().IsSub);
    }

    [Fact]
    public async Task GetSubstitutes_RepositoryReturnsSubs_ReturnsSubs()
    {
        // Arrange
        var expected = new List<Person> { new() { Id = 42, IsSub = true } };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetFilteredAsync(A<System.Linq.Expressions.Expression<Func<Person, bool>>>._)).Returns(expected);

        // Act
        var result = await service.GetSubstitutesAsync();

        // Assert
        var actual = result.Value;
        Assert.Single(actual);
        Assert.True(actual.First().IsSub);
    }

    [Fact]
    public async Task AddPerson_ValidPerson_ReturnsTrue()
    {
        // Arrange
        var expected = new Person { Id = 67, Name = "Squeakers" };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.AddAsync(A<Person>._)).Returns(true);

        // Act
        var result = await service.AddAsync(expected);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task AddPerson_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Person { Id = 4, Name = "Mr. Big Hands" };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.AddAsync(A<Person>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.AddAsync(expected);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task DeletePerson_ValidId_ReturnsTrue()
    {
        // Arrange
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.DeleteByIdAsync(A<int>._)).Returns(true);

        // Act
        var result = await service.DeleteAsync(42);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task DeletePerson_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.DeleteByIdAsync(A<int>._)).Returns(false);

        // Act
        var result = await service.DeleteAsync(42);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task UpdatePerson_ValidPerson_ReturnsTrue()
    {
        // Arrange
        var expected = new Person { Id = 7, Name = "Mr. Grabby Hands" };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.UpdateAsync(A<Person>._)).Returns(true);

        // Act
        var result = await service.UpdateAsync(expected);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task UpdatePerson_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Person { Id = 8, Name = "Nolan" };
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.UpdateAsync(A<Person>._)).Returns(false);

        // Act
        var result = await service.UpdateAsync(expected);

        // Assert
        Assert.True(result.IsFailed);
    }

    [Fact]
    public async Task AddPerson_NullPerson_ReturnsFailedResult()
    {
        // Arrange
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);

        // Act
        var result = await service.AddAsync(null!);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Person cannot be null.", result.Errors[0].Message);
    }

    [Fact]
    public async Task UpdatePerson_NullPerson_ReturnsFailedResult()
    {
        // Arrange
        var personRepo = A.Fake<IBaseRepository<Person>>();
        var service = new PersonService(personRepo);

        // Act
        var result = await service.UpdateAsync(null!);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Person cannot be null.", result.Errors[0].Message);
    }
}