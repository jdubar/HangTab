namespace HangTab.Services;
public interface IFileSystemService
{
    Task<Stream> OpenAppPackageFileAsync(string fileName);
}
