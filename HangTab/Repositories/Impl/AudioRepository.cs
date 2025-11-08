using FluentResults;

using Plugin.Maui.Audio;

namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class AudioRepository(IAudioManager audioManager) : IAudioRepository
{
    public async Task<Result> PlayAudioStreamAsync(Stream audioStream)
    {
        if (audioStream is null)
        {
            return Result.Fail(new Error("Audio stream cannot be null"));
        }

        if (!audioStream.CanRead)
        {
            return Result.Fail(new Error("Audio stream is not readable"));
        }

        try
        {
            var player = audioManager.CreateAsyncPlayer(audioStream);
            await player.PlayAsync(CancellationToken.None);
            return Result.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail(new Error(ex.Message).CausedBy(ex));
        }
        finally
        {
            await audioStream.DisposeAsync();
        }
    }
}
