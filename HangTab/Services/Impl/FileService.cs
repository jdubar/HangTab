namespace HangTab.Services.Impl;
public class FileService : IFileService
{
    public async Task<Stream> GetStream(string filename)
    {
        return await FileSystem.OpenAppPackageFileAsync(filename);
    }
}
