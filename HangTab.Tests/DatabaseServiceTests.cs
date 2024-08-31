using HangTab.Models;
using HangTab.Services;

namespace HangTab.Tests;

public class DatabaseServiceTests
{
    [Theory]
    [ClassData(typeof(TestData.ComplexData.BowlerTheoryData))]
    public async Task ItShouldCheckIfBowlerExists(Bowler bowler, bool expected)
    {
        // Given
        var service = A.Fake<IDatabaseService>();
        A.CallTo(() => service.IsBowlerExists(bowler)).Returns(expected);

        // When
        var actual = await service.IsBowlerExists(bowler);

        // Then
        actual.Should().Be(expected);
    }
}
