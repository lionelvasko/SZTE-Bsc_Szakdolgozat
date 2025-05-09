﻿using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using SomfyAPI.Services;
using System.Globalization;
using Szakdoga.Services;
using TuyaAPI.Services;
namespace Szakdoga
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMauiBlazorWebView();

            builder.Services.AddSingleton<SomfyApiService>();
            builder.Services.AddSingleton<ShutterControl>();
            builder.Services.AddSingleton<TuyaApiService>();
            builder.Services.AddSingleton<ControlLights>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<DbService>();
            builder.Services.AddSingleton<AuthenticationService>();
            builder.Services.AddLocalization();
            builder.Services.AddSingleton<GlobalizationService>();
            builder.Services.AddSingleton<ThemeChangingService>();
            builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();

            var app = builder.Build();
            ServiceHelper.Services = app.Services;
            return app;
        }

        public static void SetCulture(string cultureCode)
        {
            var culture = new CultureInfo(cultureCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}