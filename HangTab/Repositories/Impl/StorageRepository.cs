namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class StorageRepository(IFileSystem fileSystem) : IStorageRepository
{
    public async Task<Result<Stream>> OpenAppPackageFileAsync(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return Result.Fail("File name cannot be null or empty.");
        }

        if (!await fileSystem.AppPackageFileExistsAsync(fileName))
        {
            return Result.Fail($"File not found in app package '{fileName}'.");
        }

        try
        {
            var stream = await fileSystem.OpenAppPackageFileAsync(fileName);
            return Result.Ok(stream);
        }
        catch (FileNotFoundException)
        {
            return Result.Fail($"File not found in app package '{fileName}'.");
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error opening file: {ex.Message}");
        }
    }

    public async Task<Result<string>> SaveFileAsync(FileResult result)
    {
        if (result is null)
        {
            return Result.Fail("File result cannot be null.");
        }

        return await SaveAsync(result, fileSystem.AppDataDirectory);
    }

    public async Task<Result<string>> SaveScreenshotAsync(IScreenshotResult result)
    {
        if (result is null)
        {
            return Result.Fail("Screenshot result cannot be null.");
        }

        return await SaveAsync(result, fileSystem.CacheDirectory);
    }

    private static async Task<Result<string>> SaveAsync<T>(T result, string folderPath) where T : class
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
            return Result.Ok(localFilePath);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Error saving file to disk: {ex.Message}");
        }
    }
}
