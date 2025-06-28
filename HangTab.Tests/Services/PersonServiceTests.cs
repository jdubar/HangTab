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
        var expected = new Person { Id = id, Name = "Alice" };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetPersonById(A<int>._)).Returns(Task.FromResult(expected));

        // Act
        var result = await service.GetPersonById(id);

        // Assert
        Assert.Equal(expected.Id, result.Id);
        Assert.Equal("Alice", result.Name);
        A.CallTo(() => personRepo.GetPersonById(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllPeople_RepositoryReturnsPeople_ReturnsPeople()
    {
        // Arrange
        var expected = new List<Person> { new() { Id = 16 }, new() { Id = 17 } };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetAllPeople()).Returns(Task.FromResult<IEnumerable<Person>>(expected));

        // Act
        var result = await service.GetAllPeople();

        // Assert
        Assert.Equal(expected.Count, result.Count());
        A.CallTo(() => personRepo.GetAllPeople()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetRegulars_RepositoryReturnsRegulars_ReturnsRegulars()
    {
        // Arrange
        var expected = new List<Person> { new() { Id = 42, IsSub = false } };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetRegulars()).Returns(Task.FromResult<IEnumerable<Person>>(expected));

        // Act
        var result = await service.GetRegulars();

        // Assert
        Assert.Single(result);
        Assert.False(result.First().IsSub);
        A.CallTo(() => personRepo.GetRegulars()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetSubstitutes_RepositoryReturnsSubs_ReturnsSubs()
    {
        // Arrange
        var expected = new List<Person> { new() { Id = 42, IsSub = true } };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.GetSubstitutes()).Returns(Task.FromResult<IEnumerable<Person>>(expected));

        // Act
        var result = await service.GetSubstitutes();

        // Assert
        Assert.Single(result);
        Assert.True(result.First().IsSub);
        A.CallTo(() => personRepo.GetSubstitutes()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddPerson_ValidPerson_ReturnsTrue()
    {
        // Arrange
        var expected = new Person { Id = 67, Name = "Bob" };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.AddPerson(A<Person>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.AddPerson(expected);

        // Assert
        Assert.True(result);
        A.CallTo(() => personRepo.AddPerson(expected)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddPerson_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Person { Id = 4, Name = "Carol" };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.AddPerson(A<Person>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.AddPerson(expected);

        // Assert
        Assert.False(result);
        A.CallTo(() => personRepo.AddPerson(expected)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeletePerson_ValidId_ReturnsTrue()
    {
        // Arrange
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.DeletePerson(A<int>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.DeletePerson(5);

        // Assert
        Assert.True(result);
        A.CallTo(() => personRepo.DeletePerson(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeletePerson_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.DeletePerson(A<int>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.DeletePerson(6);

        // Assert
        Assert.False(result);
        A.CallTo(() => personRepo.DeletePerson(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdatePerson_ValidPerson_ReturnsTrue()
    {
        // Arrange
        var expected = new Person { Id = 7, Name = "Stimpy" };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.UpdatePerson(A<Person>._)).Returns(Task.FromResult(true));

        // Act
        var result = await service.UpdatePerson(expected);

        // Assert
        Assert.True(result);
        A.CallTo(() => personRepo.UpdatePerson(A<Person>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdatePerson_RepositoryReturnsFalse_ReturnsFalse()
    {
        // Arrange
        var expected = new Person { Id = 8, Name = "Frank" };
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.UpdatePerson(A<Person>._)).Returns(Task.FromResult(false));

        // Act
        var result = await service.UpdatePerson(expected);

        // Assert
        Assert.False(result);
        A.CallTo(() => personRepo.UpdatePerson(A<Person>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddPerson_NullPerson_ThrowsArgumentNullException()
    {
        // Arrange
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.AddPerson(A<Person>._)).Throws(new ArgumentNullException(nameof(Person)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.AddPerson(null!));
    }

    [Fact]
    public async Task UpdatePerson_NullPerson_ThrowsArgumentNullException()
    {
        // Arrange
        var personRepo = A.Fake<IPersonRepository>();
        var service = new PersonService(personRepo);
        A.CallTo(() => personRepo.UpdatePerson(A<Person>._)).Throws(new ArgumentNullException(nameof(Person)));

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdatePerson(null!));
    }
}