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
                }
            };

            return window;
        }
    }
}
