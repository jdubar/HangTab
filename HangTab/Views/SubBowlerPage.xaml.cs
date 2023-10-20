using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SubBowlerPage : ContentPage
{
    private readonly SubsViewModel _viewModel;

    public SubBowlerPage(SubsViewModel viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
	}

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadSubBowlersAsync();
    }

}