using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Services;
using HangTab.ViewModels.Base;

using Plugin.Maui.BottomSheet.Navigation;

namespace HangTab.ViewModels.BottomSheets;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class AvatarSelectViewModel(
    IDialogService dialogService,
    IMediaPickerService mediaPickerService,
    IMessenger messenger,
    IBottomSheetNavigationService bottomSheetNavigationService) : ViewModelBase
{
    [RelayCommand]
    private async Task DeleteBowlerImageAsync()
    {
        if (await dialogService.Ask("Delete", "Remove the bowler's profile image?"))
        {
            await bottomSheetNavigationService.GoBackAsync();
            messenger.Send(new PersonImageAddedOrChangedMessage(null));
        }
    }

    [RelayCommand]
    private async Task SelectBowlerImageFromCameraAsync()
    {
        var photoPath = await mediaPickerService.TakePhotoAsync();
        if (string.IsNullOrEmpty(photoPath))
        {
            return;
        }

        await bottomSheetNavigationService.GoBackAsync();
        messenger.Send(new PersonImageAddedOrChangedMessage(photoPath));
    }

    [RelayCommand]
    private async Task SelectBowlerImageFromGalleryAsync()
    {
        var photoPath = await mediaPickerService.PickPhotoAsync();
        if (string.IsNullOrEmpty(photoPath))
        {
            return;
        }

        await bottomSheetNavigationService.GoBackAsync();
        messenger.Send(new PersonImageAddedOrChangedMessage(photoPath));
    }
}
