using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class MediaPickerService(IMediaPickerRepository mediaPickerRepository) : IMediaPickerService
{
    public async Task<string> PickPhotoAsync() => await mediaPickerRepository.PickPhotoAsync();
    public async Task<string> TakePhotoAsync() => await mediaPickerRepository.TakePhotoAsync();
}
