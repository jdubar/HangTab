using HangTab.Repositories;
using HangTab.Services.Impl;

namespace HangTab.Tests.Services;
public class ScreenshotServiceTests
{
    [Fact]
    public async Task TakeScreenshotAsync_Successfully_ReturnsFilePath()
    {
        // Arrange
        var expected = "path/to/screenshot.png";
        var screenshotRepo = A.Fake<IScreenshotRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new ScreenshotService(screenshotRepo, storageRepo);
        A.CallTo(() => screenshotRepo.TakeScreenshotAsync()).Returns(A.Fake<IScreenshotResult>());
        A.CallTo(() => storageRepo.SaveScreenshotAsync(A<IScreenshotResult>._)).Returns(expected);

        // Act
        var actual = await service.TakeScreenshotAsync();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task TakeScreenshotAsync_SaveFails_ReturnsEmpty()
    {
        // Arrange
        var expected = string.Empty;
        var screenshotRepo = A.Fake<IScreenshotRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new ScreenshotService(screenshotRepo, storageRepo);
        A.CallTo(() => screenshotRepo.TakeScreenshotAsync()).Returns(A.Fake<IScreenshotResult>());
        A.CallTo(() => storageRepo.SaveScreenshotAsync(A<IScreenshotResult>._)).Returns(expected);

        // Act
        var actual = await service.TakeScreenshotAsync();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task TakeScreenshotAsync_OnNull_ReturnsEmpty()
    {
        // Arrange
        var expected = string.Empty;
        var screenshotRepo = A.Fake<IScreenshotRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new ScreenshotService(screenshotRepo, storageRepo);
        A.CallTo(() => screenshotRepo.TakeScreenshotAsync()).Returns((IScreenshotResult?)null);

        // Act
        var actual = await service.TakeScreenshotAsync();

        // Assert
        Assert.Equal(expected, actual);
    }
}
