using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using SomfyAPI.Services;
using TuyaAPI.Services;
using Szakdoga.Services;
using System.Globalization;
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
            builder.Services.AddSingleton<TuyaApiService>();
            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddScoped<AuthenticationService>();

            builder.Services.AddLocalization();
            builder.Services.AddSingleton<LocalizationService>();

            builder.Services.AddSingleton<ThemeChangingService>();

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
            builder.Services.AddAuthorizationCore();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            var app = builder.Build();

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