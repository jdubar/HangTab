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
}