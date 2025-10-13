namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class StorageRepository(IFileSystem fileSystem) : IStorageRepository
{
    public async Task<string> SaveFileAsync(FileResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return await SaveAsync(result, fileSystem.AppDataDirectory);
    }

    public async Task<string> SaveScreenshotAsync(IScreenshotResult result)
    {
        ArgumentNullException.ThrowIfNull(result);
        return await SaveAsync(result, fileSystem.CacheDirectory);
    }

    private static async Task<string> SaveAsync<T>(T result, string folderPath) where T : class
    {
        string fileName;
        Stream sourceStream;

        switch (result)
        {
            case FileResult fileResult:
                fileName = fileResult.FileName;
                sourceStream = await fileResult.OpenReadAsync();
                break;
            case IScreenshotResult screenshotResult:
                fileName = "screenshot.jpg";
                sourceStream = await screenshotResult.OpenReadAsync();
                break;
            default:
                throw new ArgumentException("Unsupported type", nameof(result));
        }

        var localFilePath = Path.Combine(folderPath, fileName);

        try
        {
            using var localFileStream = File.OpenWrite(localFilePath);

            await sourceStream.CopyToAsync(localFileStream);
            return localFilePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving file to disk: {ex.Message}");
            return string.Empty;
        }
    }
}
