namespace HangTab.Messages;
public class BowlerHangCountChangedMessage(int id, int hangCount)
{
    public int Id { get; set; } = id;
    public int HangCount { get; set;} = hangCount;
}
