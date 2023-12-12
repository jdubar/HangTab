using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SettingsPage : ContentPage
{
    private readonly BowlerViewModel _viewModel;

    public SettingsPage(BowlerViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private void ClearAllDataOnTap(object sender, TappedEventArgs e)
    {
        _viewModel.DropAllTablesCommand.ExecuteAsync(null);
    }
}