using FluentResults;

namespace HangTab.Services;
public interface IMediaService
{
    Task<Result<string>> PickPhotoAsync();
}
