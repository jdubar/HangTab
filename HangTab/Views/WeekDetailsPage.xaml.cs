using HangTab.Views.ViewModels;

namespace HangTab.Views;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We won't test UI code-behind.")]
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