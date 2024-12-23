using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.ViewModels;
public partial class BowlerViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _firstName;

    [ObservableProperty]
    private string _lastName;

    [ObservableProperty]
    private string _imageUrl;

    [ObservableProperty]
    private bool _isSub;

    public bool Equals(BowlerViewModel? other)
    {
        if (other is null)
        {
            return false;
        }
        return Id == other.Id;
    }
}
