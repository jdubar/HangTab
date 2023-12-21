using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class AddBowlerPage : ContentPage
{
    public AddBowlerPage(ManageBowlerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}