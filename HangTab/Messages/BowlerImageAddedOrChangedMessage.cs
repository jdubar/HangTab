namespace HangTab.Messages;
public class BowlerImageAddedOrChangedMessage(string imageUrl)
{
    public string ImageUrl { get; } = imageUrl;
}
