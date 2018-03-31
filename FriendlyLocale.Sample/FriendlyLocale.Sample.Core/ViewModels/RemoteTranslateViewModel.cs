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
                    {"ru", "https://nofile.io/g/2MQvPCOsAfTRc2ktw7vwi8lqVB6jeQqncYHDCc0piPC6mQ31RogkNGzHxgeTrWNF/ru.yaml/"},
                    {"en", "https://nofile.io/g/NXa51XbbY3q54Mv3WUie9Ei0dMmKzSrMWYUmsbTPdiL4m1idnBMBGyGbl1I0BHvu/en.yaml/"}
                }
            };

            // assembly offline locale
            // var assembly = this.GetType().GetTypeInfo().Assembly;
            // var offlineConfig = OfflineContentConfig.FromAssembly(assembly, "ru.yaml", "Locales");

            // local offline locale
            var offlineConfig = OfflineContentConfig.FromLocal("en.yaml", "Locales");

            FriendlyLocale.I18N.Initialize(remoteConfig, offlineConfig);
            this.ReloadItems();
        }

        protected override LocaleCellElement CreateLocaleCellElement(ILocale locale)
        {
            return locale is RemoteLocale remoteLocale ? new RemoteLocaleCellElement(remoteLocale) : base.CreateLocaleCellElement(locale);
        }
    }
}