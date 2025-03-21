namespace HangTab.Messages;
public record BowlerAddedOrChangedMessage(int Id = 0, bool IsSub = false);
public record BowlerDeletedMessage(int Id);
public record BowlerHangCountChangedMessage(int Id, int HangCount);
public record BowlerImageAddedOrChangedMessage(string ImageUrl);
public record WeekBusRideCountChangedMessage(int BusRideCount = 0);