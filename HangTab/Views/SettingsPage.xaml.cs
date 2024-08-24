using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class SettingsPage
{
    private readonly SettingsViewModel _viewModel;

    public SettingsPage(SettingsViewModel viewModel)
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

    private void ClearAllDataOnTap(object sender, EventArgs e) =>
        _viewModel.DropAllTablesCommand.Execute(null);

    private void ResetHangingsOnTap(object sender, EventArgs e) =>
        _viewModel.ResetAllHangingsCommand.Execute(null);

    private void SaveSeasonSettingsOnChange(object sender, EventArgs e) =>
        _viewModel?.UpdateSeasonSettingsCommand.Execute(null);
}