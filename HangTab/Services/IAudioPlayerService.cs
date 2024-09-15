namespace HangTab.Services;
public interface IAudioPlayerService
{
    void Play(string name, Stream audioStream);

    event Action AudioEnded;
}
