namespace HangTab.Repositories;
public interface IMediaPickerRepository
{
    Task<FileResult?> PickPhotoAsync();
    Task<FileResult?> TakePhotoAsync();
}
