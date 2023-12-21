using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class SwitchBowlerPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public SwitchBowlerPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSwitchBowlersAsync();
    }
}