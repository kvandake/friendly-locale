namespace FriendlyLocale.Configs
{
    using System.Collections.Generic;

    public class RemoteContentConfig
    {
        private IDictionary<string, string> locales;

        public string CacheDir { get; set; } = string.Empty;

        public string CacheFilePrefix { get; set; } = "friendly";

        public IDictionary<string, string> Locales
        {
            get => this.locales ?? (this.locales = new Dictionary<string, string>());
            set => this.locales = value;
        }
    }
}