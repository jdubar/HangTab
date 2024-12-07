using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Messages.Errors;

namespace HangTab.Views.ViewModels;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
[QueryProperty(nameof(Bowler), nameof(Bowler))]
public partial class AddBowlerViewModel(
    IBowlerService bowlerService,
    IShellService shellService,
    IMediaService mediaService) : BaseViewModel
{
    [ObservableProperty]
    private Bowler _bowler;

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await shellService.DisplayPromptAsync("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            if (!await bowlerService.Delete(id))
            {
                await shellService.DisplayAlertAsync("Delete Error", "Bowler was not deleted", "Ok");
                return;
            }
            await shellService.DisplayToastAsync("Bowler deleted");
            await shellService.ReturnToPageAsync();
        }
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        if (string.IsNullOrEmpty(Bowler.FirstName))
        {
            await shellService.DisplayAlertAsync("Validation Error", "First name is required.", "Ok");
            return;
        }

        if (Bowler.Id == 0 && await bowlerService.Exists(Bowler))
        {
            await shellService.DisplayAlertAsync("Validation Error", "This bowler already exists", "Ok");
            return;
        }

        if (!(Bowler.Id == 0
                ? await bowlerService.Add(Bowler)
                : await bowlerService.Update(Bowler)))
        {
            await shellService.DisplayAlertAsync("Update Error", "Unable to save bowler", "Ok");
            return;
        }

        await shellService.DisplayToastAsync("Bowler saved");
        await shellService.ReturnToPageAsync();
    }

    [RelayCommand]
    private async Task SelectBowlerImageAsync()
    {
        var result = await mediaService.PickPhotoAsync();
        if (result.HasError<PickPhotoCanceled>())
        {
            return;
        }

        if (result.IsFailed)
        {
            await shellService.DisplayAlertAsync("Error", result.Errors[0].Message, "Ok");
            return;
        }

        Bowler.ImageUrl = result.Value;
    }
}
