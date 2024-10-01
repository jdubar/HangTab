using Plugin.Maui.Audio;

namespace HangTab.Services.Impl;
public class AudioPlayerService(IAudioManager audioManager) : IAudioPlayerService
{
    private readonly Dictionary<string, IAudioPlayer> _players = new();
    private string _currentAudio = string.Empty;

    public event Action AudioEnded;

    public void Play(string name, Stream audioStream)
    {
        if (!_players.TryGetValue(name, out var audioPlayer))
        {
            audioPlayer = audioManager.CreatePlayer(audioStream);
            _players[name] = audioPlayer;
        }
        
        var playing = _players.FirstOrDefault(p => p.Value.IsPlaying).Value;
        playing?.Stop();
        
        audioPlayer.PlaybackEnded += PlayerOnPlaybackEnded;

        audioPlayer.Play();
        _currentAudio = name;
    }

    private void PlayerOnPlaybackEnded(object sender, EventArgs e)
    {
        AudioEnded?.Invoke();
        _players[_currentAudio].PlaybackEnded -= PlayerOnPlaybackEnded;
    }
}
