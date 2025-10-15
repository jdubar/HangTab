using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class ShareService(IShareRepository share) : IShareService
{
    public async Task ShareFileAsync(string filePath) => await share.ShareFileAsync(filePath);
}
