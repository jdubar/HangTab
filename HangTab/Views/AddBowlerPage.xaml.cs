using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class AddBowlerPage : ContentPage
{
    public AddBowlerPage(AddBowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}