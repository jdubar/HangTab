using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Models.Wrappers;
public partial class BowlerWrapper : ObservableObject
{
    public BowlerWrapper(Bowler bowler)
    {
        if (bowler is null)
        {
            return;
        }

        Id = bowler.Id;
        FirstName = bowler.FirstName;
        LastName = bowler.LastName;
        ImageUrl = bowler.ImageUrl;
        IsHidden = bowler.IsHidden;
        IsSub = bowler.IsSub;
        TotalHangings = bowler.TotalHangings;
    }

    public int Id { get; set; }

    [ObservableProperty]
    private string _firstName;
    [ObservableProperty]
    private string _lastName;
    [ObservableProperty]
    private string _imageUrl;
    [ObservableProperty]
    private bool _isHidden;
    [ObservableProperty]
    private bool _isSub;
    [ObservableProperty]
    private int _totalHangings;
}
