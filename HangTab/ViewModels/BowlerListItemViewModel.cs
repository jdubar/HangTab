using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Extensions;

namespace HangTab.ViewModels;
public partial class BowlerListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _imageUrl;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private string _initials;

    public BowlerListItemViewModel(
        int id,
        string name,
        bool isSub,
        string? imageUrl = null)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
        IsSub = isSub;
        Initials = name.GetInitials();
    }
}
