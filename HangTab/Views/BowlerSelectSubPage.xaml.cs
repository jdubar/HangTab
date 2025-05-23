using HangTab.ViewModels;
using HangTab.Views.Base;

namespace HangTab.Views;

public partial class BowlerSelectSubPage : ContentPageBase
{
    public BowlerSelectSubPage(BowlerSelectSubViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}