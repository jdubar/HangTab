﻿using CommunityToolkit.Mvvm.ComponentModel;

using SQLite;

namespace HangTab.Models;

[Table("bowler")]
public class Bowler : ObservableObject
{
    private int _totalHangings;
    private string _imageUrl = "account_circle.png";

    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string ImageUrl
    {
        get => _imageUrl;
        set => SetProperty(ref _imageUrl, value);
    }

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public bool IsSub { get; set; }
    public bool IsHidden { get; set; }
    public int TotalHangings
    {
        get => _totalHangings;
        set => SetProperty(ref _totalHangings, value);
    }

    public string FullName => $"{FirstName} {LastName}";
}
