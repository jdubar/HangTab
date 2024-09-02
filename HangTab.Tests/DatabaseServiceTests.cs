using System.Linq.Expressions;
using HangTab.Data;
using HangTab.Models;
using HangTab.Services;
using HangTab.Services.Impl;

namespace HangTab.Tests;

public class DatabaseServiceTests
{
    private IDatabaseContext DatabaseContext { get; }
    private DatabaseService DatabaseService { get; }

    public DatabaseServiceTests()
    {
        DatabaseContext = A.Fake<IDatabaseContext>();
        DatabaseService = new DatabaseService(DatabaseContext);
    }

    [Theory]
    [InlineData("Donnie", "George", false)]
    [InlineData("", "", false)]
    //[ClassData(typeof(TestData.ComplexData.BowlerTheoryData))]
    public async Task ItShouldCheckIfBowlerExists(string firstName, string lastName, bool expected)
    {
        // Given
        var bowler = new Bowler()
        {
            FirstName = firstName,
            LastName = lastName
        };

        A.CallTo(() => DatabaseContext.GetFilteredAsync(A<Expression<Func<Bowler, bool>>>.Ignored))
            .As<IReadOnlyCollection<Bowler>>();

        // When
        var actual = await DatabaseService.IsBowlerExists(bowler);

        // Then
        actual.Should().Be(expected);
    }
}