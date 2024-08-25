using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

namespace HangTab.Views.ViewModels;

[QueryProperty(nameof(Bowler), nameof(Bowler))]
public partial class AddBowlerViewModel(IDatabaseService data,
                                        IShellService shell,
                                        IMediaService media) : BaseViewModel
{
    [ObservableProperty]
    private Bowler _bowler;

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await shell.DisplayPromptAsync("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (!await data.DeleteBowler(id))
                {
                    await shell.DisplayAlertAsync("Delete Error", "Bowler was not deleted", "Ok");
                    return;
                }
                await shell.DisplayToastAsync("Bowler deleted");
                await shell.ReturnToPageAsync();
            }, "Deleting bowler...");
        }
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        var (isValid, errorMessage) = Bowler.ValidateFields();
        if (!isValid)
        {
            await shell.DisplayAlertAsync("Validation Error", errorMessage, "Ok");
            return;
        }

        var busyText = "Updating bowler...";
        if (Bowler.Id == 0)
        {
            busyText = "Creating bowler...";
            if (await data.IsBowlerExists(Bowler))
            {
                await shell.DisplayAlertAsync("Validation Error", "This bowler already exists", "Ok");
                return;
            }
        }

        await ExecuteAsync(async () =>
        {
            if (!(Bowler.Id == 0
                ? await data.AddBowler(Bowler)
                : await data.UpdateBowler(Bowler)))
            {
                await shell.DisplayAlertAsync("Update Error", "Unable to save bowler", "Ok");
                return;
            }
            await shell.DisplayToastAsync("Bowler saved");
            await shell.ReturnToPageAsync();
        }, busyText);
    }

    [RelayCommand]
    private async Task SelectBowlerImageAsync()
    {
        await ExecuteAsync(async () =>
        {
            var result = await media.PickPhotoAsync();
            if (result.IsFailed)
            {
                await shell.DisplayAlertAsync("Error", result.Errors[0].Message, "Ok");
                return;
            }

            Bowler.ImageUrl = result.Value;
        }, "Setting Bowler Image");
    }
}
