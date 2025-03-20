using System.Globalization;
using System.Resources;
using Szakdoga.Resources.Localization;

namespace Szakdoga.Services
{
    public class LocalizationService
    {
        private readonly ResourceManager _resourceManager;

        public LocalizationService()
        {
            _resourceManager = new ResourceManager(typeof(AppResources));
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }

        public void SetLanguage(string languageCode)
        {
            CultureInfo culture = new CultureInfo(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}
