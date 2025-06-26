using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Extensions;

namespace HangTab.ViewModels.Items;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
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
