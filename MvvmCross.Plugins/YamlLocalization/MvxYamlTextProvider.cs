namespace YamlLocalization
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using FriendlyLocale;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Extensions;
    using MvvmCross.Localization;

    public class MvxYamlTextProvider : IMvxTextProvider, IMvxLocalizationProvider
    {
        private static II18N TextProvider => I18N.Instance;

        public MvxYamlTextProvider(AssemblyContentConfig config)
        {
            I18N.Initialize(config);
        }

        public MvxYamlTextProvider(AssetsContentConfig config)
        {
            I18N.Initialize(config);
        }

        public MvxYamlTextProvider(RemoteContentConfig config)
        {
            I18N.Initialize(config);
        }
        
        public CultureInfo CurrentCultureInfo { get; private set; }

        public async Task ChangeLocale(CultureInfo cultureInfo)
        {
            await TextProvider.ChangeLocale(cultureInfo);
            this.CurrentCultureInfo = cultureInfo;
        }

        public IEnumerable<CultureInfo> GetAvailableCultures()
        {
            return TextProvider.GetAvailableCultures();
        }

        public string GetText(string namespaceKey, string typeKey, string name)
        {
            var resolvedKey = FindResolvedKey(namespaceKey, typeKey, name);

            return TextProvider.Translate(resolvedKey);
        }

        public Task ChangeLocale(string locale)
        {
            return TextProvider.ChangeLocale(locale);
        }

        public string GetText(string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            var resolvedKey = FindResolvedKey(namespaceKey, typeKey, name);

            return TextProvider.Translate(resolvedKey, formatArgs);
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name)
        {
            textValue = this.GetText(namespaceKey, typeKey, name);
            return textValue != null;
        }

        public bool TryGetText(out string textValue, string namespaceKey, string typeKey, string name, params object[] formatArgs)
        {
            if (!this.TryGetText(out textValue, namespaceKey, typeKey, name))
            {
                return false;
            }

            // Key is found but matching value is empty. Don't format but return true.
            if (string.IsNullOrEmpty(textValue))
            {
                return true;
            }

            textValue = string.Format(textValue, formatArgs);
            return true;
        }

        private static string FindResolvedKey(string namespaceKey, string typeKey, string name)
        {
            var resolvedKey = name;

            if (!string.IsNullOrEmpty(typeKey))
            {
                resolvedKey = $"{typeKey}.{resolvedKey}";
            }

            if (!string.IsNullOrEmpty(namespaceKey))
            {
                resolvedKey = $"{namespaceKey}.{resolvedKey}";
            }

            return resolvedKey;
        }
    }
}