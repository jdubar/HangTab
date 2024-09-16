namespace HangTab.Services.Impl;
public class AudioService(IAudioPlayerService player,
                          IFileService fileService) : IAudioService
{
    public void PlayBusRideSound()
    {
        using var stream = fileService.GetStream(Constants.BusRideSoundFileName);
        player.Play(Constants.BusRideSoundFileName, stream.Result);
    }
}
