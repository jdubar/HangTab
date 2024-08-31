using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class SwitchBowlerPage
{
    private readonly SwitchBowlerViewModel _viewModel;

    public SwitchBowlerPage(SwitchBowlerViewModel viewModel)
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