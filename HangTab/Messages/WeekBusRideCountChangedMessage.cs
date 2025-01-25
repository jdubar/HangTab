namespace HangTab.Messages;
public class WeekBusRideCountChangedMessage(int busRideCount = 0)
{
    public int BusRideCount { get; } = busRideCount;
}
