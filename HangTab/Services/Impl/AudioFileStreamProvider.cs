namespace HangTab.Services.Impl;
public class AudioFileStreamProvider : IAudioFileStreamProvider
{
    public async Task<Stream> GetStream(string filename)
    {
        return await FileSystem.OpenAppPackageFileAsync(filename);
    }
}
