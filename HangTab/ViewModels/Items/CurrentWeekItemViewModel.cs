using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Enums;
using HangTab.Messages;
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
    
    partial void OnHangCountChanged(int oldValue, int newValue)
    {
        if (oldValue != newValue && newValue >= 0)
        {
            WeakReferenceMessenger.Default.Send(new BowlerHangCountChangedMessage(BowlerId, newValue));
        }
    }

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string? _imageUrl;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private string _initials;

    [ObservableProperty]
    private bool _hasLowestHangCount;

    public bool EnableStepper => Status is not Status.Blind;

    public bool ShowActiveOption => Status is Status.Blind or Status.UsingSub;
    public bool ShowBlindOption => Status is Status.Active;
    public bool IsBlind => Status is Status.Blind;

    public CurrentWeekListItemViewModel(
        int weekId,
        int bowlerId,
        int personId,
        int? subId,
        Status status,
        int hangCount,
        string name,
        bool isSub,
        string initials,
        string? imageUrl = null)
    {
        WeekId = weekId;
        BowlerId = bowlerId;
        PersonId = personId;
        SubId = subId;
        Status = status;
        HangCount = hangCount;
        Name = name;
        IsSub = isSub;
        Initials = initials;
        ImageUrl = imageUrl;
    }
}
