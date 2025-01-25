namespace HangTab.Messages;
public class BowlerAddedOrChangedMessage(int id = 0, bool isSub = false)
{
    public int Id { get; } = id;
    public bool IsSub { get; } = isSub;
}
