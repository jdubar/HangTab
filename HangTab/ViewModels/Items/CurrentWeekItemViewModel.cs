using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Enums;
using HangTab.Extensions;
using HangTab.ViewModels.Items.Interfaces;

namespace HangTab.ViewModels.Items;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class CurrentWeekListItemViewModel : ObservableObject, ILowestHangCountBowler
{
    [ObservableProperty]
    private int _weekId;

    [ObservableProperty]
    private int _bowlerId;

    [ObservableProperty]
    private int _personId;

    [ObservableProperty]
    private int? _subId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnableStepper))]
    [NotifyPropertyChangedFor(nameof(ShowActiveOption))]
    [NotifyPropertyChangedFor(nameof(ShowBlindOption))]
    [NotifyPropertyChangedFor(nameof(IsBlind))]
    private Status _status;

    [ObservableProperty]
    private int _hangCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Initials))]
    private string _name = string.Empty;

    [ObservableProperty]
    private string? _imageUrl;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private bool _hasLowestHangCount;

    public bool EnableStepper => Status is not Status.Blind;
    public bool ShowActiveOption => Status is Status.Blind or Status.UsingSub;
    public bool ShowBlindOption => Status is Status.Active;
    public bool IsBlind => Status is Status.Blind;
    public string Initials => Name.GetInitials();
}
