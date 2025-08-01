﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace HangTab.ViewModels.Items;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class WeekListItemViewModel : ObservableObject
{
    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _number;

    [ObservableProperty]
    private int _busRides;

    [ObservableProperty]
    private int _hangCount;

    public WeekListItemViewModel(
        int id,
        int number,
        int busRides,
        int hangCount)
    {
        Id = id;
        Number = number;
        BusRides = busRides;
        HangCount = hangCount;
    }
}
