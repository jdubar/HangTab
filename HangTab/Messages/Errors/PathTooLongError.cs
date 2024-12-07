namespace HangTab.Messages;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "There's no logic to test.")]
public class PathTooLongError : Error
{
    public PathTooLongError(Exception err, string filePath)
        : base($"Path is too long: '{filePath}'")
        => _ = CausedBy(err);
}