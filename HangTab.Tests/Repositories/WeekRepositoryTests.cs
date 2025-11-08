using HangTab.Data;
using HangTab.Models;
using HangTab.Repositories.Impl;

using System.Linq.Expressions;

namespace HangTab.Tests.Repositories;

public class WeekRepositoryTests
{
    private readonly IDatabaseContext _context = A.Fake<IDatabaseContext>();

    [Fact]
    public async Task GetWeekById_WithValidId_ReturnsWeekWithBowlers()
    {
        // Arrange
        var personId = 10;
        var personName = "Bowler";
        var weekId = 2;
        var week = new Week { Id = weekId, Number = 5 };
        var bowlers = new List<Bowler>
        {
            new() { Id = 1, WeekId = weekId, PersonId = personId, Status = Enums.Status.Active, HangCount = 3, SubId = null }
        };
        var person = new Person { Id = personId, Name = personName };
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);

        A.CallTo(() => context.GetItemByIdAsync<Week>(A<int>._)).Returns(week);
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>._)).Returns(bowlers);
        A.CallTo(() => context.GetItemByIdAsync<Person>(A<int>._)).Returns(person);

        // Act
        var actual = await weekRepository.GetByIdAsync(weekId);

        // Assert
        Assert.Equal(weekId, actual.Id);
        Assert.Single(actual.Bowlers);
        Assert.Equal(personId, actual.Bowlers[0].PersonId);
        Assert.Equal(personName, actual.Bowlers[0].Person.Name);
    }

    [Fact]
    public async Task GetWeekById_WithIdLessThan1_CreatesWeek()
    {
        // Arrange
        var createdWeek = new Week { Id = 1, Number = 1 };
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);
        A.CallTo(() => context.AddItemAsync(A<Week>._)).Returns(true);
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).Returns([]);
        // Simulate CreateWeek returns createdWeek
        A.CallTo(() => context.GetItemByIdAsync<Week>(A<int>._)).Returns(createdWeek);

        // Act
        var actual = await weekRepository.GetByIdAsync(0);

        // Assert
        Assert.Equal(1, actual.Number);
    }

    [Fact]
    public async Task GetWeekById_WeekNotFound_ReturnsNewWeek()
    {
        // Arrange
        var weekId = 99;
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);
        A.CallTo(() => context.GetItemByIdAsync<Week>(A<int>._)).Returns((Week?)null!);

        // Act
        var actual = await weekRepository.GetByIdAsync(weekId);

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(0, actual.Id); // new Week() default
    }

    [Fact]
    public async Task GetAllWeeks_CallsContext()
    {
        // Arrange
        var weeks = new List<Week> { new() { Id = 1 }, new() { Id = 2 } };
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);
        A.CallTo(() => context.GetAllWithChildrenAsync<Week>(null)).Returns(weeks);

        // Act
        var actual = await weekRepository.GetAllAsync();

        // Assert
        Assert.Equal(weeks, actual);
        A.CallTo(() => context.GetAllWithChildrenAsync<Week>(null)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task CreateWeek_AddsWeekAndBowlers()
    {
        // Arrange
        var people = new List<Person>
        {
            new() { Id = 10, Name = "A", IsSub = false },
            new() { Id = 11, Name = "B", IsSub = false }
        };
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);
        A.CallTo(() => context.AddItemAsync(A<Week>._)).Invokes((Week args) => args.Id = 5).Returns(true);
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).Returns(people);
        A.CallTo(() => context.AddItemAsync(A<Bowler>._)).Returns(true);

        // Act
        var actual = await weekRepository.CreateAsync(3);

        // Assert
        Assert.Equal(3, actual.Number);
        A.CallTo(() => context.AddItemAsync(A<Week>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => context.AddItemAsync(A<Bowler>._)).MustHaveHappened(2, Times.Exactly);
    }

    [Fact]
    public async Task CreateWeek_WithNoPeople_AddsWeekOnly()
    {
        // Arrange
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);
        A.CallTo(() => context.AddItemAsync(A<Week>._)).Returns(true);
        A.CallTo(() => context.GetFilteredAsync(A<Expression<Func<Person, bool>>>._)).Returns([]);

        // Act
        var actual = await weekRepository.CreateAsync(2);

        // Assert
        Assert.Equal(2, actual.Number);
        A.CallTo(() => context.AddItemAsync(A<Week>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => context.AddItemAsync(A<Bowler>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task UpdateWeek_CallsContext()
    {
        // Arrange
        var week = new Week { Id = 7 };
        var context = A.Fake<IDatabaseContext>();
        var weekRepository = new WeekRepository(context);
        A.CallTo(() => context.UpdateWithChildrenAsync(A<Week>._)).Returns(Task.CompletedTask);

        // Act
        await weekRepository.UpdateAsync(week);

        // Assert
        A.CallTo(() => context.UpdateWithChildrenAsync(A<Week>._)).MustHaveHappenedOnceExactly();
    }
}
