using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Extensions;

namespace HangTab.ViewModels.Items;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class SubListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Initials))]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private string? _imageUrl;
    
    [ObservableProperty]
    private bool _isSelected;

    [ObservableProperty]
    private bool _isDeleted;

    public string Initials => Name.GetInitials();
}
