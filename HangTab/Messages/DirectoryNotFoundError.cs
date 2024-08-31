namespace HangTab.Messages;

public class DirectoryNotFoundError : Error
{
    public DirectoryNotFoundError(Exception err, string filePath)
        : base($"Directory was not found: '{filePath}'")
        => _ = CausedBy(err);
}