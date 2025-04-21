using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using HangTab.Messages;
using HangTab.Services;
using HangTab.ViewModels.Base;

namespace HangTab.ViewModels;
public partial class AvatarSelectViewModel(
    IDialogService dialogService,
    IMediaPickerService mediaPickerService) : ViewModelBase
{
    [RelayCommand]
    private async Task DeleteBowlerImage()
    {
        if (await dialogService.Ask("Delete", "Remove the bowler's profile image?"))
        {
            WeakReferenceMessenger.Default.Send(new PersonImageAddedOrChangedMessage(null));
        }
    }

    [RelayCommand]
    private async Task SelectBowlerImageFromCamera()
    {
        var photo = await mediaPickerService.TakePhotoAsync();
        if (!string.IsNullOrEmpty(photo))
        {
            WeakReferenceMessenger.Default.Send(new PersonImageAddedOrChangedMessage(photo));
        }
    }

    [RelayCommand]
    private async Task SelectBowlerImageFromGallery()
    {
        var photo = await mediaPickerService.PickPhotoAsync();
        if (!string.IsNullOrEmpty(photo))
        {
            WeakReferenceMessenger.Default.Send(new PersonImageAddedOrChangedMessage(photo));
        }
    }
}
