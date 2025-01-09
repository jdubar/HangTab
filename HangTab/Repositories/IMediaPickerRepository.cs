namespace HangTab.Repositories;
public interface IMediaPickerRepository
{
    Task<string> PickPhotoAsync();
    Task<string> TakePhotoAsync();
}
