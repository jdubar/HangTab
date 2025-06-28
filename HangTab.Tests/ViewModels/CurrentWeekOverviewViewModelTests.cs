using HangTab.Enums;
using HangTab.Mappers;
using HangTab.Messages;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;
using HangTab.ViewModels.Items;

namespace HangTab.Tests.ViewModels;

public class CurrentWeekOverviewViewModelTests
{
    private readonly IAudioService _audioService = A.Fake<IAudioService>();
    private readonly IDialogService _dialogService = A.Fake<IDialogService>();
    private readonly INavigationService _navigationService = A.Fake<INavigationService>();
    private readonly ISettingsService _settingsService = A.Fake<ISettingsService>();
    private readonly IWeekService _weekService = A.Fake<IWeekService>();
    private readonly IBowlerService _bowlerService = A.Fake<IBowlerService>();
    private readonly IMapper<CurrentWeekListItemViewModel, Bowler> _bowlerMapper = A.Fake<IMapper<CurrentWeekListItemViewModel, Bowler>>();
    private readonly IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>> _currentWeekListItemViewModelMapper = A.Fake<IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>>>();

    private CurrentWeekOverviewViewModel CreateVm() =>
        new(_audioService, _dialogService, _navigationService, _settingsService, _weekService, _bowlerService, _bowlerMapper, _currentWeekListItemViewModelMapper);

    [Fact]
    public async Task LoadAsync_WhenNoBowlers_LoadsCurrentWeekAndInitializes()
    {
        // Arrange
        var vm = CreateVm();
        var week = new Week { Id = 1, Number = 2, BusRides = 3, Bowlers = [] };
        A.CallTo(() => _settingsService.CurrentWeekPrimaryKey).Returns(1);
        A.CallTo(() => _weekService.GetWeekById(1)).Returns(Task.FromResult(week));
        A.CallTo(() => _settingsService.TotalSeasonWeeks).Returns(34);

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Equal(1, vm.Id);
        Assert.Equal(2, vm.WeekNumber);
        Assert.Equal(3, vm.BusRides);
        Assert.Equal("Week 2 of 34", vm.PageTitle);
    }

    [Fact]
    public async Task SetBowlerStatusToActive_UpdatesBowlerAndReloads()
    {
        // Arrange
        var vm = CreateVm();
        // Fix for CS7036: Update the instantiation of CurrentWeekListItemViewModel to include all required constructor parameters.
        // Fix for CS0117: Replace 'Status.Inactive' with a valid value from the 'Status' enum.

        var bowlerVm = new CurrentWeekListItemViewModel(
            weekId: 1,
            bowlerId: 1,
            personId: 0,
            subId: null,
            status: Status.Blind, // Replace 'Inactive' with a valid enum value, e.g., 'Blind'.
            hangCount: 0,
            name: "Test Bowler",
            isSub: false,
            initials: "TB",
            imageUrl: null
        );
        var bowler = new Bowler { Id = 1 };
        A.CallTo(() => _bowlerMapper.Map(bowlerVm)).Returns(bowler);
        A.CallTo(() => _bowlerService.UpdateBowler(bowler)).Returns(Task.FromResult(true));
        A.CallTo(() => _weekService.GetWeekById(A<int>._)).Returns(Task.FromResult(new Week { Id = 1, Number = 1, Bowlers = [] }));
        A.CallTo(() => _settingsService.CurrentWeekPrimaryKey).Returns(1);
        A.CallTo(() => _settingsService.TotalSeasonWeeks).Returns(34);

        // Act
        await vm.SetBowlerStatusToActiveCommand.ExecuteAsync(bowlerVm);

        // Assert
        A.CallTo(() => _bowlerService.UpdateBowler(bowler)).MustHaveHappenedOnceExactly();
        Assert.Equal(Status.Active, bowlerVm.Status);
    }

    [Fact]
    public async Task SubmitWeek_ConfirmsAndCreatesNewWeek()
    {
        // Arrange
        var vm = CreateVm();
        var newWeek = new Week { Id = 2, Number = 2, Bowlers = [] };
        A.CallTo(() => _dialogService.Ask(A<string>._, A<string>._, A<string>._, A<string>._)).Returns(Task.FromResult(true));
        A.CallTo(() => _weekService.CreateWeek(A<int>._)).Returns(Task.FromResult(newWeek));
        A.CallTo(() => _weekService.GetWeekById(A<int>._)).Returns(Task.FromResult(newWeek));
        A.CallTo(() => _settingsService.CurrentWeekPrimaryKey).Returns(1);
        A.CallTo(() => _settingsService.TotalSeasonWeeks).Returns(34);

        // Act
        await vm.SubmitWeekCommand.ExecuteAsync(null);

        // Assert
        A.CallTo(() => _weekService.CreateWeek(A<int>._)).MustHaveHappened();
        A.CallTo(() => _weekService.GetWeekById(A<int>._)).MustHaveHappened();
    }

    [Fact]
    public void Receive_PersonDeletedMessage_RemovesBowler()
    {
        // Arrange
        var vm = CreateVm();
        var bowlerVm = new CurrentWeekListItemViewModel(
            weekId: 1,
            bowlerId: 5,
            personId: 5,
            subId: null,
            status: Status.Active,
            hangCount: 0,
            name: "Test Bowler",
            isSub: false,
            initials: "TB",
            imageUrl: null
        );
        vm.CurrentWeekBowlers = [bowlerVm];

        // Act
        vm.Receive(new PersonDeletedMessage(5));

        // Assert
        Assert.Empty(vm.CurrentWeekBowlers);
    }

    [Fact]
    public async Task Receive_BowlerHangCountChangedMessage_UpdatesBowlerAndTeamHangTotal()
    {
        // Arrange
        var vm = CreateVm();
        var bowlerVm = new CurrentWeekListItemViewModel(
            weekId: 1,
            bowlerId: 7,
            personId: 0,
            subId: null,
            status: Status.Active,
            hangCount: 2,
            name: "Test Bowler",
            isSub: false,
            initials: "TB",
            imageUrl: null
        );
        vm.CurrentWeekBowlers = [bowlerVm];
        var bowler = new Bowler { Id = 7 };
        A.CallTo(() => _bowlerMapper.Map(bowlerVm)).Returns(bowler);
        A.CallTo(() => _bowlerService.UpdateBowler(bowler)).Returns(Task.FromResult(true));

        // Act
        vm.TeamHangTotal = 2;
        await Task.Run(() => vm.Receive(new BowlerHangCountChangedMessage(7, 3)));

        // Assert
        A.CallTo(() => _bowlerService.UpdateBowler(bowler)).MustHaveHappened();
        Assert.Equal(2, vm.TeamHangTotal);
    }
}