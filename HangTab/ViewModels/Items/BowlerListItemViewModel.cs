using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Enums;
using HangTab.Extensions;
using HangTab.Messages;

namespace HangTab.ViewModels.Items;
public partial class BowlerListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isSub;

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
    private int _bowlerId;

    [ObservableProperty]
    private string? _imageUrl;
    
    [ObservableProperty]
    private string _initials;

    [ObservableProperty]
    private bool _hasLowestHangCount;

    [ObservableProperty]
    private Status _status;

    public BowlerListItemViewModel(
        int id,
        string name,
        bool isSub,
        int bowlerId = 0,
        int hangCount = 0,
        string? imageUrl = null,
        Status status = Status.Active)
    {
        Id = id;
        Name = name;
        IsSub = isSub;
        BowlerId = bowlerId;
        HangCount = hangCount;
        ImageUrl = imageUrl;
        Status = status;
        Initials = name.GetInitials();
    }
}
