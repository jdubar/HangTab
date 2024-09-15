namespace HangTab.Services.Impl;
public class AudioFileStreamProvider : IAudioFileStreamProvider
{
    public Task<Stream> GetStream(string filename)
    {
        return FileSystem.OpenAppPackageFileAsync(filename);
    }
}
