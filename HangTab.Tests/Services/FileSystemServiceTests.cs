using HangTab.Services.Impl;

namespace HangTab.Tests.Services;

public class FileSystemServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task OpenAppPackageFileAsync_NullOrEmptyFileName_ThrowsArgumentException(string? fileName)
    {
        // Arrange
        var fileSystem = A.Fake<IFileSystem>();
        var service = new FileSystemService(fileSystem);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.OpenAppPackageFileAsync(fileName!));
        Assert.Equal("File name cannot be null or empty. (Parameter 'fileName')", ex.Message);
    }

    [Fact]
    public async Task OpenAppPackageFileAsync_FileExists_ReturnsStream()
    {
        // Arrange
        var expected = new MemoryStream();
        var fileSystem = A.Fake<IFileSystem>();
        var service = new FileSystemService(fileSystem);
        A.CallTo(() => fileSystem.OpenAppPackageFileAsync(A<string>._)).Returns(Task.FromResult<Stream>(expected));

        // Act
        var actual = await service.OpenAppPackageFileAsync("file.txt");

        // Assert
        Assert.Same(expected, actual);
        A.CallTo(() => fileSystem.OpenAppPackageFileAsync(A<string>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task OpenAppPackageFileAsync_FileNotFound_ThrowsFileNotFoundException()
    {
        // Arrange
        var fileName = "missing.txt";
        var fileSystem = A.Fake<IFileSystem>();
        var service = new FileSystemService(fileSystem);
        A.CallTo(() => fileSystem.OpenAppPackageFileAsync(A<string>._)).Returns(Task.FromResult<Stream>(null!));

        // Act & Assert
        var ex = await Assert.ThrowsAsync<FileNotFoundException>(() => service.OpenAppPackageFileAsync(fileName));
        Assert.Equal("File not found in app package.", ex.Message);
        Assert.Equal(fileName, ex.FileName);
    }
}