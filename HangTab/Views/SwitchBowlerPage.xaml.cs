using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SwitchBowlerPage : ContentPage
{
    private readonly BowlerViewModel _viewModel;

    public SwitchBowlerPage(BowlerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadHiddenBowlersAsync();
    }
}