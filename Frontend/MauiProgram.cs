using Microsoft.AspNetCore.Components.Authorization;
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

            builder.Services.AddScoped<SomfyApiService>();
            builder.Services.AddScoped<ShutterControl>();
            builder.Services.AddScoped<TuyaApiService>();
            builder.Services.AddScoped<ControlLights>();
            builder.Services.AddSingleton<HttpClient>();

            builder.Services.AddScoped<DbService>();
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