using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.Models.ViewModels;

public class BowlerViewModel : ObservableObject
{
    private bool _isEnableSwitchBowler = true;
    private bool _isEnableUndo;
    private bool _isLowestHangs;

    public Bowler Bowler { get; set; } = new();
    public BowlerWeek BowlerWeek { get; init; } = new();
    public bool IsEnableSwitchBowler
    {
        get => _isEnableSwitchBowler;
        set => SetProperty(ref _isEnableSwitchBowler, value);
    }
    public bool IsEnableUndo
    {
        get => _isEnableUndo;
        set => SetProperty(ref _isEnableUndo, value);
    }
    public bool IsLowestHangs
    {
        get => _isLowestHangs;
        set => SetProperty(ref _isLowestHangs, value);
    }
}
