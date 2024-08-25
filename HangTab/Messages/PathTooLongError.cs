namespace HangTab.Messages;

public class PathTooLongError : Error
{
    public PathTooLongError(Exception err, string filePath)
        : base($"Path is too long: '{filePath}'")
        => _ = CausedBy(err);
}