namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class ShareRepository(IShare share) : IShareRepository
{
    public async Task ShareFileAsync(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            return;
        }

        await share.RequestAsync(new ShareFileRequest
        {
            Title = "Share file",
            File = new ShareFile(filePath)
        });
    }
}
