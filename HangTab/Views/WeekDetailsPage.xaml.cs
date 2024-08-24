using HangTab.Views.ViewModels;

namespace HangTab.Views;

public partial class WeekDetailsPage
{
    private readonly WeekDetailsViewModel _viewModel;

    public WeekDetailsPage(WeekDetailsViewModel viewModel)
	{
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;

    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InitializeDataCommand.Execute(null);
    }
}