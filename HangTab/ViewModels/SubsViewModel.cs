using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using HangTab.Data;
using HangTab.Models;

using System.Collections.ObjectModel;

namespace HangTab.ViewModels
{
    public partial class SubsViewModel : BaseViewModel
    {
        private readonly DatabaseContext _context;

        public SubsViewModel(DatabaseContext context)
        {
            _context = context;
        }

        [ObservableProperty]
        private ObservableCollection<Bowler> _subs;


        [ObservableProperty]
        private Bowler _operatingBowler = new();

        [RelayCommand]
        public async Task LoadSubBowlersAsync()
        {
            await ExecuteAsync(async () =>
            {
                SetOperatingBowlerCommand.Execute(new());
                Subs ??= new ObservableCollection<Bowler>();
                var subs = await _context.GetFilteredAsync<Bowler>(b => b.IsSub);
                if (subs is not null && subs.Any())
                {
                    Subs.Clear();

                    foreach (var sub in subs)
                    {
                        Subs.Add(sub);
                    }
                }
            }, "Loading subs...");
        }

        [RelayCommand]
        private void SetOperatingBowler(Bowler? bowler) =>
            OperatingBowler = bowler ?? new();
    }
}
