namespace FriendlyLocale
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FriendlyLocale.Interfaces;

    public interface II18N
    {
        ILocale CurrentLocale { get; }

        string FallbackLocale { get; set; }
        
        string Translate(string[] innerKeys, string fallback = default(string));

        string Translate(string key, string fallback = default(string));

        string Translate(string key, params object[] args);

        IEnumerable<ILocale> GetAvailableLocales();

        ILocale GetLocale(string locale);

        Task ChangeLocale(ILocale locale, IProgress<float> progress = null);

        Task ChangeLocale(string locale, IProgress<float> progress = null);
    }
}