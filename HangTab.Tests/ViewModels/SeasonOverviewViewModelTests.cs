using HangTab.Mappers;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;
using HangTab.ViewModels.Items;

namespace HangTab.Tests.ViewModels;

public class SeasonOverviewViewModelTests
{
    private readonly IWeekService _weekService = A.Fake<IWeekService>();
    private readonly ISettingsService _settingsService = A.Fake<ISettingsService>();
    private readonly INavigationService _navigationService = A.Fake<INavigationService>();
    private readonly IMapper<IEnumerable<Week>, IEnumerable<WeekListItemViewModel>> _mapper = A.Fake<IMapper<IEnumerable<Week>, IEnumerable<WeekListItemViewModel>>>();

    private SeasonOverviewViewModel CreateVm() =>
        new(_weekService, _settingsService, _navigationService, _mapper);

    [Fact]
    public async Task LoadAsync_WhenWeeksExist_LoadsWeeks()
    {
        // Arrange
        var vm = CreateVm();
        var weeks = new List<Week>
        {
            new() { Id = 1, Number = 1 },
            new() { Id = 2, Number = 2 }
        };
        var weekVms = new List<WeekListItemViewModel>
        {
            new(id: 1, number: 1, busRides: 4, hangCount: 7),
            new(id: 2, number: 2, busRides: 2, hangCount: 10)
        };
        A.CallTo(() => _weekService.GetAllWeeks()).Returns(Task.FromResult<IEnumerable<Week>>(weeks));
        A.CallTo(() => _mapper.Map(A<IEnumerable<Week>>._)).Returns(weekVms);
        A.CallTo(() => _settingsService.CurrentWeekPrimaryKey).Returns(2);

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Equal(2, weekVms.Count);
        Assert.Equal(1, vm.Weeks[0].Id);
    }

    [Fact]
    public async Task LoadAsync_WhenNoWeeks_WeeksRemainsEmpty()
    {
        // Arrange
        var vm = CreateVm();
        A.CallTo(() => _weekService.GetAllWeeks()).Returns(Task.FromResult<IEnumerable<Week>>([]));

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Empty(vm.Weeks);
    }

    [Fact]
    public async Task NavigateToSelectedWeekDetailsCommand_NavigatesAndClearsSelection()
    {
        // Arrange
        var vm = CreateVm();
        var weekVm = new WeekListItemViewModel(id: 5, number: 5, busRides: 7, hangCount: 42);
        vm.SelectedWeek = weekVm;

        // Act
        await vm.NavigateToSelectedWeekDetailsCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _navigationService.GoToWeekDetails(5)).MustHaveHappenedOnceExactly();
        Assert.Null(vm.SelectedWeek);
    }

    [Fact]
    public async Task NavigateToSelectedWeekDetailsCommand_DoesNothingIfNoSelection()
    {
        // Arrange
        var vm = CreateVm();
        vm.SelectedWeek = null;

        // Act
        await vm.NavigateToSelectedWeekDetailsCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _navigationService.GoToWeekDetails(A<int>._)).MustNotHaveHappened();
        Assert.Null(vm.SelectedWeek);
    }
}