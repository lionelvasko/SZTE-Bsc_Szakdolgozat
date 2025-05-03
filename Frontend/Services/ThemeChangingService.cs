using System.Runtime.Versioning;

namespace Szakdoga.Services
{
    public class ThemeChangingService
    {
        private const string ThemeKey = "AppTheme";
        public event Action OnThemeChanged;

        public AppTheme CurrentTheme { get; private set; }

        public ThemeChangingService()
        {
            string savedTheme = Preferences.Get(ThemeKey, AppTheme.Light.ToString());
            CurrentTheme = TryParseAppTheme(savedTheme, out AppTheme theme) ? theme : AppTheme.Dark;
            ApplyTheme(CurrentTheme);
        }

        public void SetTheme(AppTheme theme)
        {
            if (CurrentTheme == theme)
                return;

            CurrentTheme = theme;
            Preferences.Set(ThemeKey, theme.ToString());
            ApplyTheme(theme);
            OnThemeChanged?.Invoke();
        }

        private static void ApplyTheme(AppTheme theme)
        {
            Application.Current!.UserAppTheme = theme;
        }

        [SupportedOSPlatform("windows10.0.17763.0")]
        private static bool TryParseAppTheme(string value, out AppTheme theme)
        {
            return Enum.TryParse(value, out theme);
        }
    }

}
