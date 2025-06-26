namespace HangTab.Messages;

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record BowlerHangCountChangedMessage(int BowlerId, int HangCount);

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record BowlerSubChangedMessage(int Id, int SubId);

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record PersonAddedOrChangedMessage(int Id = 0, bool IsSub = false);

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record PersonDeletedMessage(int Id);

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record PersonImageAddedOrChangedMessage(string? ImageUrl);

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record SystemResetMessage();

[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test messages. There's no logic to test.")]
public record WeekBusRideCountChangedMessage(int BusRideCount = 0);