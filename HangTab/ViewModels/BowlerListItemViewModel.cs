using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Extensions;

namespace HangTab.ViewModels;
public partial class BowlerListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string? _lastName;

    [ObservableProperty]
    private string? _imageUrl;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private string _fullName;

    [ObservableProperty]
    private string _initials;

    public BowlerListItemViewModel(
        int id,
        string firstName,
        bool isSub,
        string fullName,
        string? lastName = null,
        string? imageUrl = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        ImageUrl = imageUrl;
        IsSub = isSub;
        FullName = fullName;
        Initials = fullName.GetInitials();
    }
}
