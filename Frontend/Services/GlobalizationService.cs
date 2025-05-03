using System.Globalization;
using System.Resources;
using Szakdoga.Resources.Globalization;

namespace Szakdoga.Services
{
    public class GlobalizationService
    {
        private readonly ResourceManager _resourceManager;

        public GlobalizationService()
        {
            _resourceManager = new ResourceManager(typeof(AppResources));
        }

        public string GetString(string key)
        {
            return _resourceManager.GetString(key, CultureInfo.CurrentUICulture) ?? key;
        }

        public static void SetCulture(string languageCode)
        {
            CultureInfo culture = new(languageCode);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }
    }
}