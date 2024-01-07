using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("bowler")]
public class Bowler : ObservableObject
{
    private bool _isLowestHangs;
    private int _totalHangings;
    private int _weekHangings;
    private string _imageUrl = "account_circle.png";

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ImageUrl
    {
        get => _imageUrl;
        set => SetProperty(ref _imageUrl, value);
    }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsSub { get; set; }
    public bool IsHidden { get; set; }
    public int WeekHangings
    {
        get => _weekHangings;
        set => SetProperty(ref _weekHangings, value);
    }
    public int TotalHangings
    {
        get => _totalHangings;
        set => SetProperty(ref _totalHangings, value);
    }
    public bool IsLowestHangs
    {
        get => _isLowestHangs;
        set => SetProperty(ref _isLowestHangs, value);
    }

    public string FullName => $"{FirstName} {LastName}";

    public (bool IsValid, string ErrorMessage) ValidateFields()
    {
        return string.IsNullOrWhiteSpace(FirstName)
            ? ((bool IsValid, string ErrorMessage))(false, "First name is required.")
            : ((bool IsValid, string ErrorMessage))(true, string.Empty);
    }
}
