namespace FriendlyLocale.Models
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using FriendlyLocale.Impl;

    public class RemoteLocale : Locale
    {
        private string cacheSource;
        private bool downloaded;

        public RemoteLocale(string locale, string source)
            : base(locale, source)
        {
        }

        public bool Downloaded
        {
            get => this.downloaded;
            internal set
            {
                this.downloaded = value;
                this.OnPropertyChanged();
            }
        }

        internal string CacheSource => this.cacheSource ?? (this.cacheSource = this.GetCacheSource());

        public string CacheFilePrefix { get; internal set; }

        private string GetCacheSource()
        {
            var uniqueName = DecodeString(ToBase64String(RemoveSpecialCharacters(this.Source)), 8).ToString();
            return string.IsNullOrEmpty(this.CacheFilePrefix)
                ? $"{uniqueName}_{this.Key}.{I18NProvider.YamlFileExtension}"
                : $"{this.CacheFilePrefix}_{uniqueName}_{this.Key}.{I18NProvider.YamlFileExtension}";
        }

        private static string ToBase64String(string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            return Convert.ToBase64String(bytes);
        }

        // https://stackoverflow.com/a/6359285
        private static long DecodeString(string str, int baze)
        {
            long result = 0;
            var place = 1;
            for (var i = 0; i < str.Length; ++i)
            {
                result += str[str.Length - 1 - i] * place;
                place *= baze;
            }

            return result;
        }

        private static string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]", "", RegexOptions.None);
        }
    }
}