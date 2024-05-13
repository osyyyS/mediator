﻿using Shiny.Mediator.Impl;

namespace Sample;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.ConfigureContainer(new MediatorServiceProviderFactory());
        
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseShinyFramework(
                new DryIocContainerExtension(),
                prism => prism.CreateWindow("NavigationPage/MainPage"),
                new(ErrorAlertType.FullError)
            )
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterInfrastructure()
            .RegisterAppServices()
            .RegisterViews();

        return builder.Build();
    }


    static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        // register your own services here!
        return builder;
    }


    static MauiAppBuilder RegisterInfrastructure(this MauiAppBuilder builder)
    {
#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddDebug();
#endif
        var s = builder.Services;
        s.AddDataAnnotationValidation();
        return builder;
    }


    static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        var s = builder.Services;


        s.RegisterForNavigation<MainPage, MainViewModel>();
        return builder;
    }
}