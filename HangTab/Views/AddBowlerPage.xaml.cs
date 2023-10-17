using HangTab.ViewModels;

namespace HangTab.Views;

public partial class AddBowlerPage : ContentPage
{
    public AddBowlerPage(BowlersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}