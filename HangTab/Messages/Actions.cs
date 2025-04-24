namespace HangTab.Messages;
public record PersonAddedOrChangedMessage(int Id = 0, bool IsSub = false);
public record PersonDeletedMessage(int Id);
public record BowlerHangCountChangedMessage(int Id, int HangCount);
public record PersonImageAddedOrChangedMessage(string? ImageUrl);
public record WeekBusRideCountChangedMessage(int BusRideCount = 0);