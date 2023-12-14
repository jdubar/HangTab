using HangTab.ViewModels;

namespace HangTab.Views;

public partial class ManageBowlersPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public ManageBowlersPage(MainViewModel viewModel)
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