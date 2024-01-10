using HangTab.Models;

using static HangTab.Services.Impl.MediaService;

namespace HangTab.Services;
public interface IMediaService
{
    Task<PhotoResult> PickPhotoAsync();
}
