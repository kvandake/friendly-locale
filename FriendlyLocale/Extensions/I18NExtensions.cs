namespace FriendlyLocale.Extensions
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    public static class I18NExtensions
    {
        public static IEnumerable<CultureInfo> GetAvailableCultures(this II18N i18N)
        {
            var locales = i18N.GetAvailableLocales()?.ToList();
            return locales?.Select(x => new CultureInfo(x.Key)).ToList();
        }

        public static Task ChangeLocale(this II18N i18N, CultureInfo cultureInfo)
        {
            return i18N.ChangeLocale(cultureInfo.Name);
        }
    }
}