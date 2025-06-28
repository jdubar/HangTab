using Plugin.Maui.Audio;

namespace HangTab.Services.Impl;
public class AudioService(
    IAudioManager audioManager,
    IFileSystemService fileSystemService) : IAudioService
{
    public async Task PlaySoundAsync(string audioFileName)
    {
        if (string.IsNullOrWhiteSpace(audioFileName))
        {
            throw new ArgumentException("File name cannot be null or empty.", nameof(audioFileName));
        }

        using var stream = await fileSystemService.OpenAppPackageFileAsync(audioFileName);
        var player = audioManager.CreateAsyncPlayer(stream) ?? throw new InvalidOperationException("Audio player could not be created. Ensure the audio file exists and is supported.");
        await player.PlayAsync(CancellationToken.None);
    }
}
