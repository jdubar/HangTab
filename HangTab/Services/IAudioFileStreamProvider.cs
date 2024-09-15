namespace HangTab.Services;
public interface IAudioFileStreamProvider
{
    Task<Stream> GetStream(string filename);
}
