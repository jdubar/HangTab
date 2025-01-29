using HangTab.Shared;

using Plugin.Maui.Audio;

namespace HangTab.Services.Impl;
public class AudioService(IAudioManager audioManager) : IAudioService
{
    public void PlayBusRideSound()
    {
        using var stream = FileSystem.OpenAppPackageFileAsync(Constants.BusRideSoundFileName).Result;
        var player = audioManager.CreatePlayer(stream);
        player.Play();
    }
}
