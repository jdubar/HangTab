using Plugin.Maui.Audio;

namespace HangTab.Services.Impl;
public class AudioService(IAudioManager audio) : IAudioService
{
    public async Task PlayBusSound()
    {
        using var player = audio.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(Constants.BusRideSoundFileName));
        player.Play();
        await Task.Delay(2000);
    }
}
