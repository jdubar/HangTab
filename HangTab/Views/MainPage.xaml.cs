﻿using HangTab.ViewModels;

namespace HangTab.Views;

public partial class MainPage : ContentPage
{
    private readonly BowlerViewModel _viewModel;

    public MainPage(BowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMainBowlersAsync();
    }

    private void OnBusRideClicked(object sender, EventArgs e)
    {
        BowlerList.ScrollTo(0);
    }
}