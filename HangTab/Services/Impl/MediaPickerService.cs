using HangTab.Repositories;

namespace HangTab.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a simple service implementation without complex logic.")]
public class MediaPickerService(
    IMediaPickerRepository media,
    IStorageRepository storage) : IMediaPickerService
{
    public async Task<string> PickPhotoAsync()
    {
        var result = await media.PickPhotoAsync();
        return result is null
            ? string.Empty
            : await storage.SaveFileAsync(result);
    }

    public async Task<string> TakePhotoAsync()
    {
        var result = await media.TakePhotoAsync();
        return result is null
            ? string.Empty
            : await storage.SaveFileAsync(result);
    }
}
