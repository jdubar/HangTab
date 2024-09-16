namespace HangTab.Services;
public interface IFileService
{
    Task<Stream> GetStream(string filename);
}
