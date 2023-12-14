using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SettingsPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public SettingsPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private void ClearAllDataOnTap(object sender, TappedEventArgs e)
    {
        _viewModel.DropAllTablesCommand.ExecuteAsync(null);
    }
}