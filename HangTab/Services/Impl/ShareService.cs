using HangTab.Repositories;

namespace HangTab.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "There is no functionality here to test.")]
public class ShareService(IShareRepository share) : IShareService
{
    public async Task ShareFileAsync(string filePath) => await share.ShareFileAsync(filePath);
}
