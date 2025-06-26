namespace HangTab.Messages;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record BowlerHangCountChangedMessage(int BowlerId, int HangCount);
public record BowlerSubChangedMessage(int Id, int SubId);
public record PersonAddedOrChangedMessage(int Id = 0, bool IsSub = false);
public record PersonDeletedMessage(int Id);
public record PersonImageAddedOrChangedMessage(string? ImageUrl);
public record SystemResetMessage();
public record WeekBusRideCountChangedMessage(int BusRideCount = 0);