namespace HangTab.Messages;

public class PickPhotoError : Error
{
    public PickPhotoError(Exception err)
        : base("Exception occured")
        => _ = CausedBy(err);
}