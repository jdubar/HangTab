using HangTab.Mappers;
using HangTab.Models;
using HangTab.Services;
using HangTab.ViewModels;
using HangTab.ViewModels.Items;

using System.Collections.ObjectModel;

namespace HangTab.Tests.ViewModels;

public class WeekDetailsViewModelTests
{
    private readonly ISettingsService _settingsService = A.Fake<ISettingsService>();
    private readonly IWeekService _weekService = A.Fake<IWeekService>();
    private readonly IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>> _mapper = A.Fake<IMapper<IEnumerable<Bowler>, IEnumerable<CurrentWeekListItemViewModel>>>();

    private WeekDetailsViewModel CreateVm() =>
        new(_settingsService, _weekService, _mapper);

    [Fact]
    public async Task LoadAsync_WithValidId_LoadsWeekAndSetsProperties()
    {
        // Arrange
        var vm = CreateVm();
        vm.Id = 7;
        var week = new Week
        {
            Id = 7,
            Number = 3,
            BusRides = 2,
            Bowlers =
            [
                new() { Id = 1, HangCount = 4 },
                new() { Id = 2, HangCount = 5 }
            ]
        };
        var bowlerVms = new List<CurrentWeekListItemViewModel>
        {
            new(weekId: 7, bowlerId: 1, personId: 1, subId: null, status: Enums.Status.Active, hangCount: 4, name: "A", isSub: false, initials: "A", imageUrl: null),
            new(weekId: 7, bowlerId: 2, personId: 2, subId: null, status: Enums.Status.Active, hangCount: 5, name: "B", isSub: false, initials: "B", imageUrl: null)
        };
        A.CallTo(() => _weekService.GetWeekById(7)).Returns(Task.FromResult(week));
        A.CallTo(() => _mapper.Map(week.Bowlers)).Returns(bowlerVms);
        A.CallTo(() => _settingsService.TotalSeasonWeeks).Returns(10);

        // Act
        await vm.LoadAsync();

        // Assert
        Assert.Equal(7, vm.Id);
        Assert.Equal(3, vm.Number);
        Assert.Equal(2, vm.BusRides);
        Assert.Equal(2, vm.Bowlers.Count);
        Assert.Equal(9, vm.TeamHangTotal);
        Assert.Equal("Week 3 of 10", vm.PageTitle);
    }

    [Fact]
    public async Task LoadAsync_WithIdZero_DoesNotCallGetWeek()
    {
        // Arrange
        var vm = CreateVm();
        vm.Id = 0;

        // Act
        await vm.LoadAsync();

        // Assert
        A.CallTo(() => _weekService.GetWeekById(A<int>._)).MustNotHaveHappened();
        // TeamHangTotal and PageTitle should still be set
        Assert.Equal(0, vm.TeamHangTotal);
        Assert.NotNull(vm.PageTitle);
    }

    [Fact]
    public void ApplyQueryAttributes_SetsId()
    {
        // Arrange
        var vm = CreateVm();
        var query = new Dictionary<string, object> { { "weekId", 42 } };

        // Act
        vm.ApplyQueryAttributes(query);

        // Assert
        Assert.Equal(42, vm.Id);
    }

    [Fact]
    public void MapWeekData_MapsAllProperties()
    {
        // Arrange
        var vm = CreateVm();
        var week = new Week
        {
            Id = 5,
            Number = 2,
            BusRides = 1,
            Bowlers =
            [
                new() { Id = 1, HangCount = 3 }
            ]
        };
        var bowlerVms = new List<CurrentWeekListItemViewModel>
        {
            new(weekId: 5, bowlerId: 1, personId: 1, subId: null, status: Enums.Status.Active, hangCount: 3, name: "A", isSub: false, initials: "A", imageUrl: null)
        };
        A.CallTo(() => _mapper.Map(week.Bowlers)).Returns(bowlerVms);

        // Act
        var mapWeekDataMethod = typeof(WeekDetailsViewModel).GetMethod("MapWeekData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        mapWeekDataMethod.Invoke(vm, [week]);

        // Assert
        Assert.Equal(5, vm.Id);
        Assert.Equal(2, vm.Number);
        Assert.Equal(1, vm.BusRides);
        Assert.Single(vm.Bowlers);
    }

    [Fact]
    public void InitializeCurrentWeekPageSettings_SetsTeamHangTotalAndPageTitle()
    {
        // Arrange
        var vm = CreateVm();
        vm.Number = 4;
        vm.Bowlers =
        [
            new(weekId: 1, bowlerId: 1, personId: 1, subId: null, status: Enums.Status.Active, hangCount: 2, name: "A", isSub: false, initials: "A", imageUrl: null),
            new(weekId: 1, bowlerId: 2, personId: 2, subId: null, status: Enums.Status.Active, hangCount: 3, name: "B", isSub: false, initials: "B", imageUrl: null)
        ];
        A.CallTo(() => _settingsService.TotalSeasonWeeks).Returns(12);

        // Act
        var initMethod = typeof(WeekDetailsViewModel).GetMethod("InitializeCurrentWeekPageSettings", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
        initMethod.Invoke(vm, null);

        // Assert
        Assert.Equal(5, vm.TeamHangTotal);
        Assert.Equal("Week 4 of 12", vm.PageTitle);
    }
}