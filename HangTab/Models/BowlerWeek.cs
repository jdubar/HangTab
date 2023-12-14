﻿using SQLite;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HangTab.Models;

[Table("bowlerweek")]
public class BowlerWeek : INotifyPropertyChanged
{
    private int _hangings;

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int WeekNumber { get; set; }
    public int BowlerId { get; set; }
    public int Hangings
    {
        get => _hangings;
        set
        {
            _hangings = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
