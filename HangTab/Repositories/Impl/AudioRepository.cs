using FluentResults;

using Plugin.Maui.Audio;

namespace HangTab.Repositories.Impl;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a Repository for the data layer and does not require unit tests.")]
public class AudioRepository(IAudioManager audioManager) : IAudioRepository
{
    public async Task<Result> PlayAudioStreamAsync(Stream audioStream)
    {
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
