using HangTab.Models;

namespace HangTab.Services;
public interface IMediaService
{
    Task<PhotoResult> PickPhotoAsync();
}
