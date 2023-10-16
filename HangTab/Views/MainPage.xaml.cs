using HangTab.ViewModels;

namespace HangTab.Views;

public partial class MainPage : ContentPage
{
    private readonly BowlersViewModel _viewModel;
    private int count = 0;

    public MainPage(BowlersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadBowlersAsync();
    }

    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //    count++;

    //    CounterBtn.Text = count == 1
    //        ? $"Clicked {count} time"
    //        : $"Clicked {count} times";

    //    SemanticScreenReader.Announce(CounterBtn.Text);
    //}
}