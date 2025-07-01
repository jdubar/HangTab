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
    IBottomSheetNavigationService bottomSheetNavigationService) : ViewModelBase
{
    [RelayCommand]
    private async Task DeleteBowlerImage()
    {
        if (await dialogService.Ask("Delete", "Remove the bowler's profile image?"))
        {
            await bottomSheetNavigationService.GoBackAsync();
            WeakReferenceMessenger.Default.Send(new PersonImageAddedOrChangedMessage(null));
        }
    }

    [RelayCommand]
    private async Task SelectBowlerImageFromCamera()
    {
        var photo = await mediaPickerService.TakePhotoAsync();
        if (!string.IsNullOrEmpty(photo))
        {
            await bottomSheetNavigationService.GoBackAsync();
            WeakReferenceMessenger.Default.Send(new PersonImageAddedOrChangedMessage(photo));
        }
    }

    [RelayCommand]
    private async Task SelectBowlerImageFromGallery()
    {
        var photo = await mediaPickerService.PickPhotoAsync();
        if (!string.IsNullOrEmpty(photo))
        {
            await bottomSheetNavigationService.GoBackAsync();
            WeakReferenceMessenger.Default.Send(new PersonImageAddedOrChangedMessage(photo));
        }
    }
}
