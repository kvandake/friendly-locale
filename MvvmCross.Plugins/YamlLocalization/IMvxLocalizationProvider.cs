namespace YamlLocalization
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    public interface IMvxLocalizationProvider
    {        
        CultureInfo CurrentCultureInfo { get; }
        
        Task ChangeLocale(CultureInfo cultureInfo);

        IEnumerable<CultureInfo> GetAvailableCultures();
    }
}