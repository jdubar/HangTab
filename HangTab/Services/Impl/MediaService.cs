namespace HangTab.Services.Impl;
public class MediaService : IMediaService
{
    public async Task<Result<string>> PickPhotoAsync()
    {
        var result = await MediaPickerWrapper.PickPhotoAsync();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var filePathResult = await MediaPickerWrapper.SavePhotoAsync(result.Value, FileSystem.CacheDirectory);
        return filePathResult.IsFailed
            ? filePathResult.ToResult()
            : Result.Ok(filePathResult.Value);
    }
}
