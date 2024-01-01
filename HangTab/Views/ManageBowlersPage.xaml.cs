using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class ManageBowlersPage : ContentPage
{
    private readonly ManageBowlerViewModel _viewModel;

    public ManageBowlersPage(ManageBowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeData();
    }
}