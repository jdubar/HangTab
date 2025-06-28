using HangTab.Repositories;

namespace HangTab.Services.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a simple service implementation without complex logic.")]
public class MediaPickerService(IMediaPickerRepository mediaPickerRepository) : IMediaPickerService
{
    public async Task<string> PickPhotoAsync() => await mediaPickerRepository.PickPhotoAsync();
    public async Task<string> TakePhotoAsync() => await mediaPickerRepository.TakePhotoAsync();
}
