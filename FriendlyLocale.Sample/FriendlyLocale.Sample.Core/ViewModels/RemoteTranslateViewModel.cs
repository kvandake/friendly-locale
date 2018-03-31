namespace FriendlyLocale.Sample
{
    using System.Reflection;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Interfaces;
    using FriendlyLocale.Models;
    using FriendlyLocale.Sample.Models;

    public class RemoteTranslateViewModel : BaseTranslateViewModel
    {
        public RemoteTranslateViewModel()
        {
            this.Title = "Remote";
        }

        protected override void ReloadLocale()
        {
            var remoteConfig = new RemoteContentConfig
            {
                Locales =
                {
                    {"ru", "https://raw.githubusercontent.com/kvandake/friendly-locale/master/FriendlyLocale.Sample/FriendlyLocale.Sample.Core/Locales/ru.yaml"},
                    {"en", "https://raw.githubusercontent.com/kvandake/friendly-locale/master/FriendlyLocale.Sample/FriendlyLocale.Sample.Core/Locales/en.yaml"}
                }
            };

            // assembly offline locale
            // var assembly = this.GetType().GetTypeInfo().Assembly;
            // var offlineConfig = OfflineContentConfig.FromAssembly(assembly, "ru.yaml", "Locales");

            // assets offline locale
            var offlineConfig = OfflineContentConfig.FromAssets("en.yaml", "Locales");

            FriendlyLocale.I18N.Initialize(remoteConfig, offlineConfig);
            this.ReloadItems();
        }

        protected override LocaleCellElement CreateLocaleCellElement(ILocale locale)
        {
            return locale is RemoteLocale remoteLocale ? new RemoteLocaleCellElement(remoteLocale) : base.CreateLocaleCellElement(locale);
        }
    }
}