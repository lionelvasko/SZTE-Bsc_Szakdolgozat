using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Szakdoga.Services
{
    using Microsoft.JSInterop;

    public class ThemeChangingService
    {
        private const string ThemeKey = "AppTheme";
        public event Action? OnThemeChanged;

        public AppTheme CurrentTheme { get; private set; }

        public ThemeChangingService()
        {
            string savedTheme = Preferences.Get(ThemeKey, AppTheme.Light.ToString());
            CurrentTheme = Enum.TryParse(savedTheme, out AppTheme theme) ? theme : AppTheme.Light;
            ApplyTheme(CurrentTheme);
        }

        public void SetTheme(AppTheme theme)
        {
            if (CurrentTheme == theme)
                return;

            CurrentTheme = theme;
            Preferences.Set(ThemeKey, theme.ToString()); // Tárolás helyileg
            ApplyTheme(theme);
            OnThemeChanged?.Invoke(); // Értesítjük a feliratkozott komponenseket
        }

        private void ApplyTheme(AppTheme theme)
        {
            Application.Current!.UserAppTheme = theme;
        }
    }

}
