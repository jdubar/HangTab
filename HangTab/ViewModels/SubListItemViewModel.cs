using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Extensions;

namespace HangTab.ViewModels;
public partial class SubListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private string? _imageUrl;
    
    [ObservableProperty]
    private string _initials;

    [ObservableProperty]
    private bool _isSelected;

    public SubListItemViewModel(
        int id,
        string name,
        bool isSub,
        string? imageUrl = null)
    {
        Id = id;
        Name = name;
        IsSub = isSub;
        ImageUrl = imageUrl;
        Initials = name.GetInitials();
    }
}
