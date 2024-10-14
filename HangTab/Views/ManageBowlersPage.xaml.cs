using HangTab.Views.ViewModels;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
public partial class ManageBowlersPage
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