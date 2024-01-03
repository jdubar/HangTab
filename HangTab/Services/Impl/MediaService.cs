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

            var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using var sourceStream = await photo.OpenReadAsync();
            using var localFileStream = File.OpenWrite(localFilePath);

            await sourceStream.CopyToAsync(localFileStream);
            result.IsSuccess = true;
            result.Result = localFilePath;
        }
        catch (Exception ex)
        {
            result.Result = ex.Message;
        }
        return result;
    }

    public class PhotoResult
    {
        public bool IsSuccess { get; set; }
        public string Result { get; set; }
    }
}
