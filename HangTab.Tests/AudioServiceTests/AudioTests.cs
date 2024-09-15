using HangTab.Services.Impl;

using Plugin.Maui.Audio;

namespace HangTab.Tests.AudioServiceTests;
public class AudioTests
{
    private IAudioManager AudioManagerFake { get; }
    private AudioPlayerService AudioService { get; }

    public AudioTests()
    {
        AudioManagerFake = A.Fake<IAudioManager>();
        AudioService = new AudioPlayerService(AudioManagerFake);
    }

    [Fact]
    public void ItShouldPlayTheBusRideSound()
    {
        // Given
        var fakePlayer = A.Fake<IAudioPlayer>();
        A.CallTo(() => AudioManagerFake.CreatePlayer(A<MemoryStream>.Ignored, null)).Returns(fakePlayer);

        var handler = A.Fake<Action>();
        AudioService.AudioEnded += handler;
        AudioService.Play("test.mp3", new MemoryStream());

        // When
        fakePlayer.PlaybackEnded += Raise.WithEmpty();

        // Then
        A.CallTo(() => handler.Invoke()).MustHaveHappened();
    }
}
