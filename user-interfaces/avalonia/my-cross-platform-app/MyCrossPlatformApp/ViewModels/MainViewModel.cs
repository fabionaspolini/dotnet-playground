﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace MyCrossPlatformApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _greeting = "Welcome to Avalonia!";
}
