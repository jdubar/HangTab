namespace HangTab.Messages;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "There's no logic to test.")]
public class DirectoryNotFoundError : Error
{
    public DirectoryNotFoundError(Exception err, string filePath)
        : base($"Directory was not found: '{filePath}'")
        => _ = CausedBy(err);
}