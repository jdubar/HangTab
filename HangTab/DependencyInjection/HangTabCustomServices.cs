﻿using HangTab.Data;
using HangTab.Data.Impl;
using HangTab.Services.Impl;

namespace HangTab.DependencyInjection;
public static class HangTabCustomServices
{
    public static void AddCustomServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

        builder.Services.AddSingleton<IAudioPlayerService, AudioPlayerService>();
        builder.Services.AddSingleton<IAudioService, AudioService>();

        builder.Services.AddSingleton<IFileService, FileService>();
        builder.Services.AddSingleton<IMediaService, MediaService>();
        builder.Services.AddSingleton<IShellService, ShellService>();
    }
}
