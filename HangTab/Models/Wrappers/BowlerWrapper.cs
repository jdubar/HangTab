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
        FullName = bowler.FullName;
        ImageUrl = bowler.ImageUrl;
        BowlerByWeekId = bowler.LineupId;
    }

    public int Id { get; }

    [ObservableProperty]
    private string _firstName;
    [ObservableProperty]
    private string _lastName;
    [ObservableProperty]
    private string _fullName;
    [ObservableProperty]
    private string _imageUrl;
    [ObservableProperty]
    private int _bowlerByWeekId;
}
