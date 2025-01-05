using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SettingsPage : ContentPageBase
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    private void ClearAllDataOnTap(object sender, EventArgs e) => _viewModel.DropAllTablesCommand.Execute(null);

    private void ResetHangingsOnTap(object sender, EventArgs e) => _viewModel.ResetAllHangingsCommand.Execute(null);

    private void SaveSeasonSettingsOnChange(object sender, EventArgs e) => _viewModel?.UpdateSeasonSettingsCommand.Execute(null);
}