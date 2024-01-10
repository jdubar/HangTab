using HangTab.Models;

namespace HangTab.Services.Impl;
public class MediaService : IMediaService
{
    public async Task<PhotoResult> PickPhotoAsync()
    {
        var result = new PhotoResult()
        {
            IsSuccess = false
        };
        try
        {
            var photo = await MediaPicker.Default.PickPhotoAsync();
            var localFilePath = SaveFileToCacheDir(photo);
            result.IsSuccess = true;
            result.Result = localFilePath.Result;
        }
        catch (Exception ex)
        {
            result.Result = ex.Message;
        }
        return result;
    }

    public async Task<PhotoResult> TakePhotoAsync()
    {
        var result = new PhotoResult()
        {
            IsSuccess = false
        };
        if (MediaPicker.Default.IsCaptureSupported)
        {
            try
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();
                var localFilePath = SaveFileToCacheDir(photo);
                result.IsSuccess = true;
                result.Result = localFilePath.Result;
            }
            catch (Exception ex)
            {
                result.Result = ex.Message;
            }
            return result;
        }
        else
        {
            result.IsSuccess = false;
            result.Result = "Capture is not supported on this device.";
            return result;
        }
    }

    private async Task<string> SaveFileToCacheDir(FileResult file)
    {
        var localFilePath = Path.Combine(FileSystem.CacheDirectory, file.FileName);
        using var sourceStream = await file.OpenReadAsync();
        using var localFileStream = File.OpenWrite(localFilePath);

        await sourceStream.CopyToAsync(localFileStream);
        return localFilePath;
    }
}
