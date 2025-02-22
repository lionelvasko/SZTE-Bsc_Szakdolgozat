using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using SomfyAPI.Services;
using Szakdoga.Services;
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
                builder.Services.AddSingleton<SomfyApiService>();
                builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddMauiBlazorWebView();


            builder.Services.AddScoped<JwtAuthenticationStateProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<JwtAuthenticationStateProvider>());
            builder.Services.AddAuthorizationCore();


#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
