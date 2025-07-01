using HangTab.Data;
using HangTab.Models;
using HangTab.Repositories.Impl;

using System.Linq.Expressions;

namespace HangTab.Tests.Repositories;

public class PersonRepositoryTests
{
    private readonly IDatabaseContext _context = A.Fake<IDatabaseContext>();

    private PersonRepository CreateRepo() => new(_context);

    [Fact]
    public async Task GetPersonById_CallsContextWithId()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        var expected = new Person { Id = 1, Name = "Test" };
        A.CallTo(() => context.GetItemByIdAsync<Person>(A<int>._)).Returns(expected);

        // Act
        var actual = await personRepository.GetPersonById(1);

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetItemByIdAsync<Person>(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetAllPeople_CallsContext()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        var expected = new List<Person> { new() { Id = 1, Name = "A" } };
        A.CallTo(() => context.GetAllAsync<Person>()).Returns(expected);

        // Act
        var actual = await personRepository.GetAllPeople();

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetAllAsync<Person>()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetRegulars_CallsContextWithCorrectPredicate()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        var expected = new List<Person> { new() { Id = 2, Name = "Reg", IsSub = false } };
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).Returns(expected);

        // Act
        var actual = await personRepository.GetRegulars();

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetSubstitutes_CallsContextWithCorrectPredicate()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        var expected = new List<Person> { new() { Id = 3, Name = "Sub", IsSub = true } };
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).Returns(expected);

        // Act
        var actual = await personRepository.GetSubstitutes();

        // Assert
        Assert.Equal(expected, actual);
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task AddPerson_CallsContextAndReturnsResult()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        var expected = new Person { Id = 4, Name = "Add" };
        A.CallTo(() => context.AddItemAsync(A<Person>._)).Returns(true);

        // Act
        var actual = await personRepository.AddPerson(expected);

        // Assert
        Assert.True(actual);
        A.CallTo(() => context.AddItemAsync(A<Person>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeletePerson_CallsContextAndReturnsResult()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        A.CallTo(() => context.DeleteItemByIdAsync<Person>(A<int>._)).Returns(true);

        // Act
        var actual = await personRepository.DeletePerson(5);

        // Assert
        Assert.True(actual);
        A.CallTo(() => context.DeleteItemByIdAsync<Person>(A<int>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task UpdatePerson_CallsContextAndReturnsResult()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var personRepository = new PersonRepository(context);
        var expected = new Person { Id = 6, Name = "Update" };
        A.CallTo(() => context.UpdateItemAsync(A<Person>._)).Returns(true);

        // Act
        var actual = await personRepository.UpdatePerson(expected);

        // Assert
        Assert.True(actual);
        A.CallTo(() => context.UpdateItemAsync(A<Person>._)).MustHaveHappenedOnceExactly();
    }
}