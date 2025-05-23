using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Enums;
using HangTab.Messages;

namespace HangTab.ViewModels;
public partial class CurrentWeekListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _weekId;

    [ObservableProperty]
    private int _bowlerId;

    [ObservableProperty]
    private int _personId;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EnableStepper))]
    [NotifyPropertyChangedFor(nameof(ShowActiveOption))]
    [NotifyPropertyChangedFor(nameof(ShowBlindOption))]
    private Status _status;

    [ObservableProperty]
    private int _hangCount;
    
    partial void OnHangCountChanged(int oldValue, int newValue)
    {
        if (oldValue != newValue && newValue >= 0)
        {
            WeakReferenceMessenger.Default.Send(new BowlerHangCountChangedMessage(PersonId, newValue));
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

    public bool EnableStepper => Status is not Status.Blind;

    public bool ShowActiveOption => Status is Status.Blind or Status.UsingSub;
    public bool ShowBlindOption => Status is Status.Active;

    public CurrentWeekListItemViewModel(
        int weekId,
        int bowlerId,
        int personId,
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
        Status = status;
        HangCount = hangCount;
        Name = name;
        IsSub = isSub;
        Initials = initials;
        ImageUrl = imageUrl;
    }
}
