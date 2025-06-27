using HangTab.Services;
using HangTab.ViewModels;

namespace HangTab.Tests.ViewModels;

public class SettingsViewModelTests
{
    private readonly IDatabaseService _databaseService = A.Fake<IDatabaseService>();
    private readonly IDialogService _dialogService = A.Fake<IDialogService>();
    private readonly ISettingsService _settingsService = A.Fake<ISettingsService>();
    private readonly IThemeService _themeService = A.Fake<IThemeService>();

    private SettingsViewModel CreateVm() =>
        new(_databaseService, _dialogService, _settingsService, _themeService);

    [Fact]
    public async Task LoadAsync_InitializesSettings()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _settingsService.TotalSeasonWeeks).Returns(34);
        A.CallTo(() => _settingsService.Theme).Returns(1); // Assume 1 = Dark

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Equal(34, vm.TotalSeasonWeeks);
        Assert.True(vm.DarkThemeEnabled);
    }

    [Fact]
    public void Setting_TotalSeasonWeeks_UpdatesSettingsService()
    {
        // Arrange
        var expected = 22;
        var vm = CreateVm();

        // Act
        vm.TotalSeasonWeeks = expected;

        // Assert
        A.CallToSet(() => _settingsService.TotalSeasonWeeks).To(expected).MustHaveHappened();
    }

    [Fact]
    public void Setting_DarkThemeEnabled_True_SetsDarkTheme()
    {
        // Arrange
        var expected = 1;
        var vm = CreateVm();
        A.CallTo(() => _settingsService.Theme).Returns(expected); // 1 = Dark

        // Act
        vm.DarkThemeEnabled = true;

        // Assert
        A.CallToSet(() => _settingsService.Theme).To(expected).MustHaveHappened(); // 1 = Dark
        A.CallTo(() => _themeService.SetDarkTheme()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllDataAsync_Confirmed_DeletesDataAndShowsToast()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(true);
        A.CallTo(() => _databaseService.DeleteAllData()).Returns(true);

        // Act
        await vm.DeleteAllDataCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _databaseService.DeleteAllData()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _dialogService.ToastAsync(A<string>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllDataAsync_Confirmed_DeleteFails_ShowsError()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(true);
        A.CallTo(() => _databaseService.DeleteAllData()).Returns(false);

        // Act
        await vm.DeleteAllDataCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _dialogService.AlertAsync("Critical Error", A<string>._, "Ok")).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task DeleteAllDataAsync_NotConfirmed_DoesNothing()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(false);

        // Act
        await vm.DeleteAllDataCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _databaseService.DeleteAllData()).MustNotHaveHappened();
    }

    [Fact]
    public async Task StartNewSeasonAsync_Confirmed_DeletesSeasonDataAndShowsToast()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(true);
        A.CallTo(() => _databaseService.DeleteSeasonData()).Returns(true);

        // Act
        await vm.StartNewSeasonCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _databaseService.DeleteSeasonData()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _dialogService.ToastAsync(A<string>._)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task StartNewSeasonAsync_Confirmed_DeleteFails_ShowsError()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(true);
        A.CallTo(() => _databaseService.DeleteSeasonData()).Returns(false);

        // Act
        await vm.StartNewSeasonCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _dialogService.AlertAsync("Critical Error", A<string>._, "Ok")).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task StartNewSeasonAsync_NotConfirmed_DoesNothing()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(false);

        // Act
        await vm.StartNewSeasonCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _databaseService.DeleteSeasonData()).MustNotHaveHappened();
    }
}