using HangTab.ViewModels;

namespace HangTab.Views;

public partial class AddBowlerPage : ContentPage
{
    public AddBowlerPage(BowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}