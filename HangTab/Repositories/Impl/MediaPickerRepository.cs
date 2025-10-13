namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class MediaPickerRepository(IMediaPicker mediaPicker) : IMediaPickerRepository
{
    public async Task<FileResult?> PickPhotoAsync()
    {
        return await mediaPicker.PickPhotoAsync(new MediaPickerOptions
        {
            Title = "Select a photo"
        });
    }

    public async Task<FileResult?> TakePhotoAsync()
    {
        return mediaPicker.IsCaptureSupported
            ? await mediaPicker.CapturePhotoAsync()
            : null;
    }
}
