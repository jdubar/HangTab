using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Popups;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class PopupViewModelBase(IPopupService popupService) : ObservableObject
{
    [RelayCommand]
    public void Cancel() => popupService.ClosePopup();

    [RelayCommand]
    public void Confirm() => popupService.ClosePopup(true);
}
