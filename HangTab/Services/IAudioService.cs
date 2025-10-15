using FluentResults;

namespace HangTab.Services;
public interface IAudioService
{
    Task<Result> PlaySoundAsync(string audioFileName);
}
