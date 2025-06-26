using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Popups;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "This is a ViewModel for the UI and does not require unit tests.")]
public partial class BowlerTypePopupViewModel(IPopupService popupService) : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RadioRegularOption))]
    private bool _radioSubstituteOption;

    public bool RadioRegularOption => !RadioSubstituteOption;

    [RelayCommand]
    public void SaveBowlerType() => popupService.ClosePopup(RadioSubstituteOption);
}
