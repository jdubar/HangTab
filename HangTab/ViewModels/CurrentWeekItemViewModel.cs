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
    private int _weeklyLineupId;

    [ObservableProperty]
    private int _bowlerId;

    [ObservableProperty]
    private BowlerStatus _status;

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

    public CurrentWeekListItemViewModel(
        int weekId,
        int weeklyLineupId,
        int bowlerId,
        BowlerStatus status,
        int hangCount,
        string name,
        string imageUrl,
        bool isSub,
        string initials)
    {
        WeekId = weekId;
        WeeklyLineupId = weeklyLineupId;
        BowlerId = bowlerId;
        Status = status;
        HangCount = hangCount;
        Name = name;
        ImageUrl = imageUrl;
        IsSub = isSub;
        Initials = initials;
    }
}
