using HangTab.Services;
using HangTab.Services.Impl;

using Plugin.Maui.Audio;

namespace HangTab.Tests.Services;

public class AudioServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task PlaySoundAsync_NullOrEmptyFileName_ThrowsArgumentException(string? fileName)
    {
        // Arrange
        var audioManager = A.Fake<IAudioManager>();
        var fileSystemService = A.Fake<IFileSystemService>();
        var service = new AudioService(audioManager, fileSystemService);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.PlaySoundAsync(fileName!));
        Assert.Equal("File name cannot be null or empty. (Parameter 'audioFileName')", ex.Message);
    }

    [Fact]
    public async Task PlaySoundAsync_PlayerIsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var audioManager = A.Fake<IAudioManager>();
        var fileSystemService = A.Fake<IFileSystemService>();
        var service = new AudioService(audioManager, fileSystemService);
        A.CallTo(() => fileSystemService.OpenAppPackageFileAsync(A<string>._)).Returns(Task.FromResult<Stream>(new MemoryStream()));
        A.CallTo(() => audioManager.CreateAsyncPlayer(A<MemoryStream>._, null)).Throws<InvalidOperationException>();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.PlaySoundAsync("bad.mp3"));
        Assert.Equal("Operation is not valid due to the current state of the object.", ex.Message);
    }
}