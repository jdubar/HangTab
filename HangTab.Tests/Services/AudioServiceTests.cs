using FluentResults;

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
        A.CallTo(() => audioRepo.PlayAudioStreamAsync(A<Stream>._)).Throws<InvalidOperationException>();

        // Act & Assert
        var actual = await service.PlaySoundAsync("sound.mp3");
        Assert.False(actual.IsSuccess);
    }

    [Fact]
    public async Task PlaySoundAsync_OpenAppPackageFile_ShouldFail()
    {
        // Arrange
        var audioRepo = A.Fake<IAudioRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new AudioService(audioRepo, storageRepo);
        A.CallTo(() => storageRepo.OpenAppPackageFileAsync(A<string>._)).Returns(Result.Fail<Stream>("File not found"));

        // Act
        var actual = await service.PlaySoundAsync("nonexistent.mp3");

        // Assert
        Assert.True(actual.IsFailed);
        A.CallTo(() => audioRepo.PlayAudioStreamAsync(A<Stream>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task PlaySoundAsync_OpenAppPackageFile_ShouldSucceed()
    {
        // Arrange
        var audioRepo = A.Fake<IAudioRepository>();
        var storageRepo = A.Fake<IStorageRepository>();
        var service = new AudioService(audioRepo, storageRepo);
        var fakeStream = new MemoryStream();
        A.CallTo(() => storageRepo.OpenAppPackageFileAsync(A<string>._)).Returns(Result.Ok<Stream>(fakeStream));
        A.CallTo(() => audioRepo.PlayAudioStreamAsync(A<Stream>._)).Returns(Result.Ok());

        // Act
        var actual = await service.PlaySoundAsync("sound.mp3");

        // Assert
        Assert.True(actual.IsSuccess);
        A.CallTo(() => audioRepo.PlayAudioStreamAsync(fakeStream)).MustHaveHappenedOnceExactly();
    }
}