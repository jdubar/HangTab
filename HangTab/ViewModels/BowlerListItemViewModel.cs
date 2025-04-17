using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Enums;
using HangTab.Extensions;
using HangTab.Messages;

namespace HangTab.ViewModels;
public partial class BowlerListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private bool _isInactive;

    [ObservableProperty]
    private bool _isSub;

    [ObservableProperty]
    private int _hangings;

    [ObservableProperty]
    private int _bowlerId;

    [ObservableProperty]
    private string? _imageUrl;
    
    [ObservableProperty]
    private string _initials;

    partial void OnHangingsChanged(int oldValue, int newValue)
    {
        if (oldValue != newValue && newValue >= 0)
        {
            WeakReferenceMessenger.Default.Send(new BowlerHangCountChangedMessage(Id, newValue));
        }
    }

    [ObservableProperty]
    private Status _status;

    public BowlerListItemViewModel(
        int id,
        string name,
        bool isInactive,
        bool isSub,
        int bowlerId = 0,
        int hangings = 0,
        string? imageUrl = null,
        Status status = Status.Active)
    {
        Id = id;
        Name = name;
        IsInactive = isInactive;
        IsSub = isSub;
        BowlerId = bowlerId;
        Hangings = hangings;
        ImageUrl = imageUrl;
        Status = status;
        Initials = name.GetInitials();
    }
}
