using HangTab.Constants;

namespace HangTab.Repositories.Impl;
public class MediaPickerRepository(IMediaPicker mediaPicker) : IMediaPickerRepository
{
    public async Task<string> PickPhotoAsync()
    {
        var photo = await MediaPicker.Default.PickPhotoAsync(new MediaPickerOptions
        {
            Title = "Select a photo"
        });
        return photo is not null
            ? await SavePhotoToDiskAsync(photo)
            : string.Empty;
    }

    public async Task<string> TakePhotoAsync()
    {
        if (!MediaPicker.Default.IsCaptureSupported)
        {
            return string.Empty;
        }

        var photo = await mediaPicker.CapturePhotoAsync();
        return photo is not null
            ? await SavePhotoToDiskAsync(photo)
            : string.Empty;
    }

    private static async Task<string> SavePhotoToDiskAsync(FileResult fileResult)
    {
        var localFilePath = Path.Combine(Files.CacheDirectory, fileResult.FileName);

        using var sourceStream = await fileResult.OpenReadAsync();
        using var localFileStream = File.OpenWrite(localFilePath);

        await sourceStream.CopyToAsync(localFileStream);
        return localFilePath;
    }
}
