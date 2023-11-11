using HangTab.ViewModels;

namespace HangTab.Views;

public partial class ManageBowlersPage : ContentPage
{
    private readonly BowlerViewModel _viewModel;

    public ManageBowlersPage(BowlerViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAllBowlersAsync();
    }
}