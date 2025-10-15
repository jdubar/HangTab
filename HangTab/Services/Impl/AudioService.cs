using FluentResults;

using HangTab.Repositories;

namespace HangTab.Services.Impl;
public class AudioService(
    IAudioRepository audioService,
    IStorageRepository storageRepository) : IAudioService
{
    public async Task<Result> PlaySoundAsync(string audioFileName)
    {
        if (string.IsNullOrWhiteSpace(audioFileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(audioFileName));
        }

        var result = await storageRepository.OpenAppPackageFileAsync(audioFileName);
        if (result.IsFailed)
        {
            return Result.Fail(result.Errors);
        }

        try
        {
            using var stream = result.Value;
            await audioService.PlayAudioStreamAsync(stream);
            return Result.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail(new Error(ex.Message).CausedBy(ex));
        }
    }
}
