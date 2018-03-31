namespace FriendlyLocale.Models
{
    using FriendlyLocale.Configs;
    using FriendlyLocale.Interfaces;

    internal class OfflineLocale : ILocale
    {
        private readonly OfflineContentConfig config;

        public OfflineLocale(OfflineContentConfig config)
        {
            this.config = config;
        }

        public string Key => "offline";
        public string DisplayName => this.Key;
        public string Source => this.config.FileName;
    }
}