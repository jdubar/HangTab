using HangTab.ViewModels;

namespace HangTab.Views;

public partial class BowlerSelectSubPage : ContentPageBase
{
    public BowlerSelectSubPage(BowlerSelectSubViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}