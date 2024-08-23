using HangTab.Models;

using Microsoft.Maui.Graphics.Platform;

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
            if (photo is null)
            {
                return result;
            }
            var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using var sourceStream = await photo.OpenReadAsync();
            var image = PlatformImage.FromStream(sourceStream);
            if (image is not null)
            {
                var newImage = image.Downsize(150, true);
                using var localFileStream = File.OpenWrite(localFilePath);
                await newImage.SaveAsync(localFileStream);
                await sourceStream.CopyToAsync(localFileStream);
            }
            result.IsSuccess = true;
            result.FilePath = localFilePath;
        }
        catch (Exception ex)
        {
            result.ErrorMsg = ex.Message;
        }
        return result;
    }
}
