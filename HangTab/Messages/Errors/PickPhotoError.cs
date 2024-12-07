namespace HangTab.Messages.Errors;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "There's no logic to test.")]
public class PickPhotoError : Error
{
    public PickPhotoError(Exception err)
        : base("Exception occured")
        => _ = CausedBy(err);
}