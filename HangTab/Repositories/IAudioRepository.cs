namespace HangTab.Repositories;
public interface IAudioRepository
{
    Task<Result> PlayAudioStreamAsync(Stream audioStream);
}
