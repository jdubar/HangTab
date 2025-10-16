using HangTab.Repositories;

namespace HangTab.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a simple service implementation without complex logic.")]
public class MediaPickerService(
    IMediaPickerRepository media,
    IStorageRepository storage) : IMediaPickerService
{
    public async Task<string> PickPhotoAsync()
    {
        var fileResult = await media.PickPhotoAsync();
        if (fileResult is null)
        {
            return string.Empty;
        }

        return await SaveFileAsync(fileResult);
    }

    public async Task<string> TakePhotoAsync()
    {
        var fileResult = await media.TakePhotoAsync();
        if (fileResult is null)
        {
            return string.Empty;
        }

        return await SaveFileAsync(fileResult);
    }

    private async Task<string> SaveFileAsync(FileResult fileResult)
    {
        var result = await storage.SaveFileAsync(fileResult);
        return result.IsSuccess
            ? result.Value
            : string.Empty;
    }
}
