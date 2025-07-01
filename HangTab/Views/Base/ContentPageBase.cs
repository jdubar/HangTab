using HangTab.ViewModels.Base;

namespace HangTab.Views.Base;
[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage(Justification = "We will not test the view code behind. There's no logic to test.")]
public partial class ContentPageBase : ContentPage
{
    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            if (BindingContext is not IViewModelBase viewModelBase)
            {
                return;
            }

            await viewModelBase.InitializeAsyncCommand.ExecuteAsync(null);
        }
        catch (Exception)
        {
            // TODO: handle exception
        }
    }
}
