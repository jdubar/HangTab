namespace HangTab.Services.Impl;
public class FileSystemService(IFileSystem fileSystem) : IFileSystemService
{
    public async Task<Stream> OpenAppPackageFileAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));
        }

        return await fileSystem.OpenAppPackageFileAsync(fileName) ?? throw new FileNotFoundException("File not found in app package.", fileName);
    }
}
