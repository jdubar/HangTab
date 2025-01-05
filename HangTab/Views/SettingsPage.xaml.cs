using HangTab.ViewModels;

namespace HangTab.Views;

public partial class SettingsPage : ContentPageBase
{
    public SettingsPage(SettingsViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}