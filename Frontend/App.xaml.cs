using System.Globalization;
using System.Net.Http.Headers;
using Szakdoga.Resources.Globalization;
using Szakdoga.Services;

namespace Szakdoga
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = new Window(new MainPage())
            {
                MinimumWidth = 500,
                MinimumHeight = 400,
                Title = "Szakdolgozat"
            };

            window.Destroying += async (s, e) =>
            {
                string? shouldRememberString = await SecureStorage.GetAsync(AuthenticationService.REMEMBER_ME_KEY);
                bool shouldRemember = bool.TryParse(shouldRememberString, out bool result) && result;
                if (!shouldRemember)
                {
                    SecureStorage.Default.Remove(AuthenticationService.JWT_AUTH_TOKEN);
                    SecureStorage.Default.Remove(AuthenticationService.REMEMBER_ME_KEY);
                    SecureStorage.Default.Remove("tuya_token");
                    SecureStorage.Default.Remove("tuya_refresh_token");
                    SecureStorage.Default.Remove("tuya_region");
                    SecureStorage.Default.Remove("somfy_token");
                    SecureStorage.Default.Remove("somfy_url");
                    SecureStorage.Default.Remove("selectedCulture");
                }
            };

            window.Created += async (s, e) =>
            {
                string? selectedCulture = await SecureStorage.GetAsync("selectedCulture");
                if (string.IsNullOrEmpty(selectedCulture))
                {
                    selectedCulture = "en-US";
                }
                CultureInfo cultureInfo = new CultureInfo(selectedCulture);
                AppResources.Culture = cultureInfo;


                string? jwt = await SecureStorage.GetAsync(AuthenticationService.JWT_AUTH_TOKEN);
                if (!string.IsNullOrEmpty(jwt))
                {
                    var client = ServiceHelper.GetService<HttpClient>();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                }
            };

            return window;
        }
    }
}
