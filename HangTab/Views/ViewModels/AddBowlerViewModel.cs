using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Models;
using HangTab.Services;

namespace HangTab.Views.ViewModels;

[QueryProperty(nameof(Bowler), nameof(Bowler))]
public partial class AddBowlerViewModel(IDatabaseService data, IShellService shell) : BaseViewModel
{
    [ObservableProperty]
    private Bowler _bowler;

    [RelayCommand]
    private async Task DeleteBowlerAsync(int id)
    {
        if (await shell.DisplayPrompt("Delete", "Are you sure you want to delete this bowler?", "Yes", "No"))
        {
            await ExecuteAsync(async () =>
            {
                if (await data.DeleteBowler(id))
                {
                    IsInitializeMainCollection = true;
                }
                else
                {
                    await shell.DisplayAlert("Delete Error", "Bowler was not deleted", "Ok");
                }
                await shell.ReturnToPage();
            }, "Deleting bowler...");
        }
    }

    [RelayCommand]
    private async Task SaveBowlerAsync()
    {
        if (Bowler is null)
        {
            return;
        }

        var (isValid, errorMessage) = Bowler.ValidateEmptyFields();
        if (!isValid)
        {
            await shell.DisplayAlert("Validation Error", errorMessage, "Ok");
            return;
        }

        var busyText = "Updating bowler...";
        if (Bowler.Id == 0)
        {
            busyText = "Creating bowler...";
            if (await data.IsBowlerExists(Bowler))
            {
                await shell.DisplayAlert("Validation Error", "This bowler already exists", "Ok");
                return;
            }
        }

        await ExecuteAsync(async () =>
        {
            if (Bowler.Id == 0
                ? await data.AddBowler(Bowler)
                : await data.UpdateBowler(Bowler))
            {
                IsInitializeMainCollection = true;
            }
            else
            {
                await shell.DisplayAlert("Update Error", "Unable to save bowler", "Ok");
            }
            await shell.ReturnToPage();
        }, busyText);
    }

    [RelayCommand]
    private async Task SelectBowlerImage()
    {
        // TODO: Finish SelectBowlerImage method
        //if (MediaPicker.Default.IsCaptureSupported)
        //{
        // Take a photo
        // var photo = await MediaPicker.Default.CapturePhotoAsync();

        // Load a photo
        var photo = await MediaPicker.Default.PickPhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                var localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                try
                {
                    await sourceStream.CopyToAsync(localFileStream);
                    Bowler.ImageUrl = localFilePath;
                }
                catch (ArgumentNullException ex)
                {
                    await shell.DisplayAlert("Error", ex.Message, "Ok");
                }
            }
        //}
        //else
        //{
        //    await shell.DisplayAlert("Error", "Device not supported", "Ok");
        //}
    }
}
