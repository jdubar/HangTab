using HangTab.Services;

namespace HangTab.Tests.FileServiceTests;
public class FileTests
{
    [Fact]
    public async Task ItShouldOpenTheStream()
    {
        // When (Static methods can not be intercepted - used a fake for the call instead)
        //      See: https://stackoverflow.com/a/5765689/6114190
        var actual = await A.Fake<IFileService>().GetStream("test.txt");

        // Then
        actual.Should().NotBeNull();
    }
}
