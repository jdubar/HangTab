namespace HangTab.Services.Impl;
public class AudioService(IAudioPlayerService player) : IAudioService
{
    public async Task PlayBusRideSound()
    {
        await using var stream = await FileSystem.Current.OpenAppPackageFileAsync(Constants.BusRideSoundFileName);
        player.Play(Constants.BusRideSoundFileName, stream);
    }
}
