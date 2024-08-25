namespace HangTab.Services.Impl;
public class MediaService : IMediaService
{
    public async Task<Result<string>> PickPhotoAsync()
    {
        var mediaPicker = new MediaPickerWrapper();
        var result = await mediaPicker.PickPhotoAsync();
        if (result.IsFailed)
        {
            return result.ToResult();
        }

        var filePathResult = await mediaPicker.SavePhotoAsync(result.Value, FileSystem.CacheDirectory);
        return filePathResult.IsFailed
            ? filePathResult.ToResult()
            : Result.Ok(filePathResult.Value);
    }
}
