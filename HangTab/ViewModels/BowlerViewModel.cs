using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Models;

namespace HangTab.ViewModels;

public class BowlerViewModel : ObservableObject
{
    private bool _isLowestHangs;

    public Bowler Bowler { get; set; } = new();
    public BowlerWeek BowlerWeek { get; set; } = new();
    public bool IsLowestHangs
    {
        get => _isLowestHangs;
        set => SetProperty(ref _isLowestHangs, value);
    }
}
