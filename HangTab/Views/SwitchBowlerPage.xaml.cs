using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SwitchBowlerPage : ContentPage
{
    private readonly BowlersViewModel _viewModel;

    public SwitchBowlerPage(BowlersViewModel viewModel)
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