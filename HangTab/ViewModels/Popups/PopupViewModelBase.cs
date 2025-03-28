using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Popups;
public partial class PopupViewModelBase(IPopupService popupService) : ObservableObject
{
    [RelayCommand]
    public void Cancel() => popupService.ClosePopup();

    [RelayCommand]
    public void Confirm() => popupService.ClosePopup(true);
}
