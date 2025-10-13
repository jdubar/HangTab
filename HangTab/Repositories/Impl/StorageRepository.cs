namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class StorageRepository(IFileSystem fileSystem) : IStorageRepository
{
    public async Task<string> SaveToDiskAsync(FileResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        var localFilePath = Path.Combine(fileSystem.CacheDirectory, result.FileName);

        try
        {
            using var sourceStream = await result.OpenReadAsync();
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
