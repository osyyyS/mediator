﻿using Microsoft.Extensions.Configuration;
using Polly;

namespace Sample;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp
            .CreateBuilder()
            .UseMauiApp<App>()
            .UsePrism(
                new DryIocContainerExtension(),
                prism => prism.CreateWindow(nav => nav
                    .CreateBuilder()
                    .AddTabbedSegment(tabs => tabs
                        .CreateTab(tab => tab
                            .AddNavigationPage()
                            .AddSegment(nameof(TriggerPage))
                        )
                        .CreateTab(tab => tab
                            .AddNavigationPage()
                            .AddSegment(nameof(EventPage))
                        )
                        .CreateTab(tab => tab
                            .AddNavigationPage()
                            .AddSegment(nameof(BlazorPage))
                        )
                    )
                )
            );

#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddDebug();
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            // NOTE: Mediator:Http is the based sub config - the namespace follows PER http generation
            // the value is the base URI
            { "Mediator:Http:Http.TheActual", "https://localhost:7192/" }
        }!);
        builder.Services.AddShinyMediator(x => x
            .UseMaui()
            .UseBlazor()
            .AddMauiHttpDecorator()
            .AddTimerRefreshStreamMiddleware()
            .AddPrismSupport()
            .AddDataAnnotations()
            
            // TODO: don't add both
            // .AddFluentValidation()
            .AddResiliencyMiddleware(
                ("Test", builder =>
                {
                    // builder.AddRetry(new RetryStrategyOptions());
                    builder.AddTimeout(TimeSpan.FromSeconds(2.0));
                })
            )
            .AddMemoryCaching(y =>
            {
                y.ExpirationScanFrequency = TimeSpan.FromSeconds(5);
            })
        );
        builder.Services.AddDiscoveredMediatorHandlersFromSample();
        
        builder.Services.AddSingleton<AppSqliteConnection>();
        builder.Services.AddMauiBlazorWebView();

        builder.Services.RegisterForNavigation<TriggerPage, TriggerViewModel>();
        builder.Services.RegisterForNavigation<EventPage, EventViewModel>();
        builder.Services.RegisterForNavigation<BlazorPage, BlazorViewModel>();
        builder.Services.RegisterForNavigation<AnotherPage, AnotherViewModel>();
        
        return builder.Build();
    }
}