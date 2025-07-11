using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Services;

namespace HangTab.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class DataManagerViewModel(
    IDialogService dialogService,
    IDatabaseService databaseService,
    ISettingsService settingsService)
{
    /// <summary>
    /// Deletes all data from the system after user confirmation.
    /// </summary>
    /// <remarks>This method prompts the user for confirmation before proceeding to delete all data.  If the
    /// user confirms, it attempts to delete the data and provides feedback on the operation's success or
    /// failure.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task completes when the deletion process and any
    /// associated user notifications are finished.</returns>
    public async Task DeleteAllDataAsync()
    {
        if (!await dialogService.Ask("Delete", "Are you sure you want to delete ALL data?", "Yes", "No"))
        {
            return;
        }

        if (await databaseService.DeleteAllData())
        {
            SendSystemResetMessage();
            await dialogService.ToastAsync("All data has been deleted");
        }
        else
        {
            await dialogService.AlertAsync("Critical Error", "Error occurred while deleting the databases!", "Ok");
        }
    }

    /// <summary>
    /// Starts a new season by resetting all bowler hangings and week data.
    /// </summary>
    /// <returns></returns>
    public async Task StartNewSeasonAsync()
    {
        if (!await dialogService.Ask("Season Reset", "Are you ready to start a new season and reset all bowler hangings?", "Yes", "No"))
        {
            return;
        }

        if (await databaseService.DeleteSeasonData())
        {
            SendSystemResetMessage();
            await dialogService.ToastAsync("A new season has started");
        }
        else
        {
            await dialogService.AlertAsync("Critical Error", "An error occurred while starting a new season!", "Ok");
        }
    }

    private void SendSystemResetMessage()
    {
        settingsService.CurrentWeekPrimaryKey = 0;
        settingsService.SeasonComplete = false;
        WeakReferenceMessenger.Default.Send(new SystemResetMessage());
    }
}
