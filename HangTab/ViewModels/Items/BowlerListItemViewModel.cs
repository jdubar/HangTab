using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Enums;
using HangTab.Extensions;
using HangTab.ViewModels.Items.Interfaces;

namespace HangTab.ViewModels.Items;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class BowlerListItemViewModel : ObservableObject, ILowestHangCountBowler
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Initials))]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private bool _isDeleted;

    [ObservableProperty]
    private int _hangCount;

    [ObservableProperty]
    private int _bowlerId;

    [ObservableProperty]
    private string? _imageUrl = null;

    public string Initials => Name.GetInitials();

    [ObservableProperty]
    private bool _hasLowestHangCount;

    [ObservableProperty]
    private Status _status = Status.Active;
}
