using HangTab.Messages;

using Microsoft.Maui.Graphics.Platform;

using Result = FluentResults.Result;

namespace HangTab.Services.Impl;

public class MediaPickerWrapper
{
    public async Task<Result<FileResult>> PickPhotoAsync()
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();
            return result is null
                ? Result.Fail("User canceled or unknown error")
                : Result.Ok(result);
        }
        catch (Exception e)
        {
            return new PickPhotoError(e);
        }
    }

    public async Task<Result<string>> SavePhotoAsync(FileResult result, string localFileSavePath, float maxWidthOrHeight = 150)
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
        return Result.Ok(localFilePath);
    }
}