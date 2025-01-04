using HangTab.ViewModels;

namespace HangTab.Models;

public partial class BowlerGroup(string name, List<BowlerListItemViewModel> bowlers) : List<BowlerListItemViewModel>(bowlers)
{
    public string GroupName { get; private set; } = name;
}