namespace HangTab.Messages;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "There's no logic to test.")]
public class FileUnauthorizedAccessError : Error
{
    public FileUnauthorizedAccessError(Exception err, string filePath)
        : base($"Unauthorized access to path: '{filePath}'")
    => _ = CausedBy(err);
}