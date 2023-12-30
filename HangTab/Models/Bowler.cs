using SQLite;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangTab.Models;

[Table("bowler")]
public class Bowler : INotifyPropertyChanged
{
    private int _totalHangings;
    private string _imageUrl;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ImageUrl
    {
        get => _imageUrl;
        set
        {
            _imageUrl = value;
            OnPropertyChanged();
        }
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsSub { get; set; }
    public bool IsHidden { get; set; }
    public int TotalHangings
    {
        get => _totalHangings;
        set
        {
            _totalHangings = value;
            OnPropertyChanged();
        }
    }

    public string FullName => $"{FirstName} {LastName}";

    public Bowler Clone() => MemberwiseClone() as Bowler;

    public (bool IsValid, string ErrorMessage) ValidateEmptyFields()
    {
        if (string.IsNullOrWhiteSpace(FirstName))
        {
            return (false, "First name is required.");
        }
        else if (string.IsNullOrWhiteSpace(LastName))
        {
            return (false, "Last name is required.");
        }
        return (true, string.Empty);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
