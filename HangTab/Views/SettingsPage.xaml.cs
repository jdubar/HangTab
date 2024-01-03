using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class SettingsPage : ContentPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
    }

    private void ClearAllDataOnTap(object sender, TappedEventArgs e)
    {
        _viewModel.DropAllTablesCommand.Execute(null);
    }
}