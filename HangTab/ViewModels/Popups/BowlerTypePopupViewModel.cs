using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Popups;
public partial class BowlerTypePopupViewModel(IPopupService popupService) : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RadioRegularOption))]
    private bool _radioSubstituteOption;

    public bool RadioRegularOption => !RadioSubstituteOption;

    [RelayCommand]
    public void SaveBowlerType() => popupService.ClosePopup(RadioSubstituteOption);
}
