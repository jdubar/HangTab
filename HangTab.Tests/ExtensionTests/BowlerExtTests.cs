using HangTab.Extensions;
using HangTab.Models;
using HangTab.Tests.TestData;
using MvvmHelpers;

namespace HangTab.Tests.ExtensionTests;
public class BowlerExtTests
{
    [Fact]
    public void ItShouldGetLowestHangBowlers()
    {
        // Given
        var bowlers = ListData.ListOfFiveBowlers;
        var expected = new List<Bowler>
        {
            new() { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png" },
            new() { Id = 2, FirstName = "Jason", LastName = "Smith", ImageUrl = "123.png" },
            new() { Id = 3, FirstName = "Kenny", LastName = "Smith", ImageUrl = "daddy.png" },
        };

        // When
        var actual = bowlers.GetLowestHangBowlers();

        // Then
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ItShouldReturnNoBowlers()
    {
        // Given
        var bowlers = new List<Bowler>
        {
            new() { Id = 4, FirstName = "Nick", LastName = "Bertus", ImageUrl = "uh-oh.png", IsSub = true },
            new() { Id = 5, FirstName = "Mike Jr.", LastName = "Fizzle", ImageUrl = "happy.png", IsSub = true }
        };

        // When
        var actual = bowlers.GetLowestHangBowlers();

        // Then
        actual.Should().BeEquivalentTo(new List<Bowler>());
    }

    [Fact]
    public void ItShouldCreateTheBowlerCollection()
    {
        // Given
        var collection = new ObservableRangeCollection<Bowler>();
        var bowlers = new List<Bowler>
        {
            new() { Id = 1, FirstName = "Joe", LastName = "Sample", ImageUrl = "abc.png" },
            new() { Id = 2, FirstName = "Jason", LastName = "Smith", ImageUrl = "123.png" },
            new() { Id = 3, FirstName = "Kenny", LastName = "Smith", ImageUrl = "daddy.png" },
            new() { Id = 4, FirstName = "Nick", LastName = "Bertus", ImageUrl = "uh-oh.png", IsSub = true },
            new() { Id = 5, FirstName = "Mike Jr.", LastName = "Fizzle", ImageUrl = "happy.png", IsSub = true }
        };
        var expected = ListData.ListOfFiveBowlers;

        // When
        collection.AddBowlersToCollection(bowlers);

        // Then
        collection.Should().BeEquivalentTo(expected);
    }
}
