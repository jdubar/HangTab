namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class MediaPickerRepository(IMediaPicker mediaPicker, IStorageRepository storageRepository) : IMediaPickerRepository
{
    public async Task<string> PickPhotoAsync()
    {
        var result = await mediaPicker.PickPhotoAsync(new MediaPickerOptions
        {
            Title = "Select a photo"
        });
        return result is not null
            ? await storageRepository.SaveToDiskAsync(result)
            : string.Empty;
    }

    public async Task<string> TakePhotoAsync()
    {
        if (!mediaPicker.IsCaptureSupported)
        {
            return string.Empty;
        }

        var result = await mediaPicker.CapturePhotoAsync();
        return result is not null
            ? await storageRepository.SaveToDiskAsync(result)
            : string.Empty;
    }
}
