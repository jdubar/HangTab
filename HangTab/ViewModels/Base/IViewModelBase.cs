using CommunityToolkit.Mvvm.Input;

namespace HangTab.ViewModels.Base;
public interface IViewModelBase
{
    public IAsyncRelayCommand InitializeAsyncCommand { get; }
}
