namespace HangTab.Messages;
public class BowlerAddedOrChangedMessage(int id = 0)
{
    public int Id { get; } = id;
}
