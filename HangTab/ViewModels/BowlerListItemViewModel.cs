using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Enums;
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

    [ObservableProperty]
    private int _hangings;

    [ObservableProperty]
    private BowlerStatus _status;

    public BowlerListItemViewModel(
        int id,
        string name,
        bool isSub,
        int hangings = 0,
        string? imageUrl = null,
        BowlerStatus status = BowlerStatus.Active)
    {
        Id = id;
        Name = name;
        ImageUrl = imageUrl;
        IsSub = isSub;
        Initials = name.GetInitials();
        Hangings = hangings;
        Status = status;
    }
}
