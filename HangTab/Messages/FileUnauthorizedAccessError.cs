namespace HangTab.Messages;

public class FileUnauthorizedAccessError : Error
{
    public FileUnauthorizedAccessError(Exception err, string filePath)
        : base($"Unauthorized access to path: '{filePath}'")
    => _ = CausedBy(err);
}