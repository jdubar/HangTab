using HangTab.Services;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var service = A.Fake<IDatabaseService>();
    }
}