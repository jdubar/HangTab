namespace HangTab.Services.Impl;
public class AudioService(IAudioFileStreamProvider streamer,
                          IAudioPlayerService player) : IAudioService
{
    public void PlayBusRideSound()
    {
        using var stream = streamer.GetStream(Constants.BusRideSoundFileName);
        player.Play(Constants.BusRideSoundFileName, stream.Result);
    }
}
