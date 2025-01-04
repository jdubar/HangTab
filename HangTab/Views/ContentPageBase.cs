using HangTab.ViewModels.Base;

namespace HangTab.Views;
public class ContentPageBase : ContentPage
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
