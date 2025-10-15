using HangTab.Repositories;
using HangTab.Services.Impl;

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
        var audioRepo = A.Fake<IAudioRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new AudioService(audioRepo, storageRepo);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.PlaySoundAsync(fileName!));
        Assert.Equal("File name cannot be null or empty. (Parameter 'audioFileName')", ex.Message);
    }

    [Fact]
    public async Task PlaySoundAsync_PlayerIsNull_ThrowsInvalidOperationException()
    {
        // Arrange
        var audioRepo = A.Fake<IAudioRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new AudioService(audioRepo, storageRepo);
        A.CallTo(() => storageRepo.OpenAppPackageFileAsync(A<string>._)).Returns(new MemoryStream());
        A.CallTo(() => audioRepo.PlayAudioStreamAsync(A<MemoryStream>._)).Throws<InvalidOperationException>();

        // Act & Assert
        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.PlaySoundAsync("bad.mp3"));
        Assert.Equal("Operation is not valid due to the current state of the object.", ex.Message);
    }
}