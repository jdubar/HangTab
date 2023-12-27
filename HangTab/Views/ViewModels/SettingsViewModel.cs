using CommunityToolkit.Mvvm.Input;

using HangTab.Services;

namespace HangTab.Views.ViewModels;
public partial class SettingsViewModel(IDatabaseService dbservice) : BaseViewModel
{
    [RelayCommand]
    private async Task DropAllTablesAsync()
    {
        if (await Shell.Current.DisplayAlert("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (!await dbservice.DropAllTables())
                {
                    await Shell.Current.DisplayAlert("Critical Error", "Error occurred while deleting data!", "Ok");
                }
            }, "Clearing all data...");
        }
    }
}
