using HangTab.ViewModels;

namespace HangTab.Views;

public partial class AddBowlerPage : ContentPage
{
    public AddBowlerPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}