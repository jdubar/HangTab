﻿using CommunityToolkit.Mvvm.ComponentModel;

using HangTab.Models;

namespace HangTab.ViewModels;

public class BowlerViewModel : ObservableObject
{
    private bool _isEnableSwitch = true;
    private bool _isEnableUndo;
    private bool _isLowestHangs;

    public Bowler Bowler { get; set; } = new();
    public BowlerWeek BowlerWeek { get; set; } = new();
    public bool IsEnableSwitch
    {
        get => _isEnableSwitch;
        set => SetProperty(ref _isEnableSwitch, value);
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
