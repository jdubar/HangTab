using Microsoft.Maui.Graphics.Platform;

namespace HangTab.Services.Impl;

public static class MediaPickerWrapper
{
    public static async Task<Result<FileResult>> PickPhotoAsync()
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();
            return result is null
                ? new PickPhotoCanceled()
                : Result.Ok(result);
        }
        catch (Exception e)
        {
            return new PickPhotoError(e);
        }
    }

    public static async Task<Result<string>> SavePhotoAsync(FileResult result, string localFileSavePath, float maxWidthOrHeight = 150)
    {
        if (result is null)
        {
            return Result.Fail("File result is null");
        }

        await using var sourceStream = await result.OpenReadAsync();
        var image = PlatformImage.FromStream(sourceStream);
        if (image is null)
        {
            return new PlatformFromImageStreamError();
        }

        var localFilePath = Path.Combine(localFileSavePath, result.FileName);
        var newImage = image.Downsize(maxWidthOrHeight, true);
        try
        {
            await using var localFileStream = File.OpenWrite(localFilePath);
            await newImage.SaveAsync(localFileStream);
            await sourceStream.CopyToAsync(localFileStream);
            return Result.Ok(localFilePath);
        }
        catch (UnauthorizedAccessException e)
        {
            return new FileUnauthorizedAccessError(e, localFilePath);
        }
        catch (PathTooLongException e)
        {
            return new PathTooLongError(e, localFilePath);
        }
        catch (DirectoryNotFoundException e)
        {
            return new DirectoryNotFoundError(e, localFilePath);
        }
    }
}