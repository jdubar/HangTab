namespace HangTab.Services;
public interface IMediaPickerService
{
    Task<string> PickPhotoAsync();
    Task<string> TakePhotoAsync();
}
