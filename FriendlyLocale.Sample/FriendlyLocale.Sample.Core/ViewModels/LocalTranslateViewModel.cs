namespace FriendlyLocale.Sample
{
    using FriendlyLocale.Configs;

    public class LocalTranslateViewModel : BaseTranslateViewModel
    {
        public LocalTranslateViewModel()
        {
            this.Title = "Local";
        }

        protected override void ReloadLocale()
        {
            var localeConfig = new LocalContentConfig
            {
                ResourceFolder = "Locales"
            };

            FriendlyLocale.I18N.Initialize(localeConfig);
            this.ReloadItems();
        }
    }
}