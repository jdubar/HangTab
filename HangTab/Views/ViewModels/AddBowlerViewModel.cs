using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HangTab.Data;

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
            if (!await data.DeleteBowler(id))
            {
                await shell.DisplayAlertAsync("Delete Error", "Bowler was not deleted", "Ok");
                return;
            }
            await shell.DisplayToastAsync("Bowler deleted");
            await shell.ReturnToPageAsync();
        }
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        if (string.IsNullOrEmpty(Bowler.FirstName))
        {
            await shell.DisplayAlertAsync("Validation Error", "First name is required.", "Ok");
            return;
        }

        if (Bowler.Id == 0 && await data.IsBowlerExists(Bowler))
        {
            await shell.DisplayAlertAsync("Validation Error", "This bowler already exists", "Ok");
            return;
        }

        if (!(Bowler.Id == 0
                ? await data.AddBowler(Bowler)
                : await data.UpdateBowler(Bowler)))
        {
            await shell.DisplayAlertAsync("Update Error", "Unable to save bowler", "Ok");
            return;
        }

        await shell.DisplayToastAsync("Bowler saved");
        await shell.ReturnToPageAsync();
    }

    [RelayCommand]
    private async Task SelectBowlerImageAsync()
    {
        var result = await media.PickPhotoAsync();
        if (result.HasError<PickPhotoCanceled>())
        {
            return;
        }

        if (result.IsFailed)
        {
            await shell.DisplayAlertAsync("Error", result.Errors[0].Message, "Ok");
            return;
        }

        Bowler.ImageUrl = result.Value;
    }
}
