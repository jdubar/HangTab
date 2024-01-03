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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InitializeDataCommand.Execute(null);
    }
}