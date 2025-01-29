using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Base;
public partial class ViewModelBase : ObservableValidator, IViewModelBase
{
    [ObservableProperty]
    private bool _isLoading;

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    public ViewModelBase()
    {
        InitializeAsyncCommand = new AsyncRelayCommand(async () =>
        {
            IsLoading = true;
            await Loading(LoadAsync);
            IsLoading = false;
        });
    }

    protected static async Task Loading(Func<Task> unitOfWork) => await unitOfWork();

    public virtual Task LoadAsync() => Task.CompletedTask;
}