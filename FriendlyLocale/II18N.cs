namespace FriendlyLocale
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FriendlyLocale.Interfaces;

    public interface II18N
    {
        /// <summary>
        ///  Current locale.
        /// </summary>
        ILocale CurrentLocale { get; }

        /// <summary>
        ///  Fallback locale.
        /// </summary>
        string FallbackLocale { get; set; }
        
        /// <summary>
        ///     Translate some key.
        /// </summary>
        /// <param name="innerKeys">nested localization keys.</param>
        /// <param name="fallback">return default value.</param>
        /// <returns>localization value.</returns>
        string Translate(string[] innerKeys, string fallback = default(string));

        /// <summary>
        ///     Translate some key.
        /// </summary>
        /// <param name="key">localization key.</param>
        /// <param name="fallback">return default value.</param>
        /// <returns>localization value.</returns>
        string Translate(string key, string fallback = default(string));

        /// <summary>
        ///     Translate some key with string format args. {0}, {1} ...
        /// </summary>
        /// <param name="key">localization key.</param>
        /// <param name="args">args for "string.Format"></param>
        /// <returns>localization value.</returns>
        string Translate(string key, params object[] args);
        
        /// <summary>
        ///      Translate some key with string naming formats. {name1}, {foo}, {bar} ...
        /// </summary>
        /// <param name="key"></param>
        /// <param name="injectionObject"></param>
        /// <returns></returns>
        string TranslateObject(string key, object injectionObject);

        /// <summary>
        ///     Translate some enum.
        /// </summary>
        /// <param name="enumValue">enum value.</param>
        /// <param name="fallback">return default value.</param>
        /// <typeparam name="TEnum">type of enum</typeparam>
        /// <returns>localization value.</returns>
        string TranslateEnum<TEnum>(TEnum enumValue, string fallback = default(string));

        /// <summary>
        ///  Get available locales
        /// </summary>
        /// <returns>available locales.</returns>
        IEnumerable<ILocale> GetAvailableLocales();

        /// <summary>
        ///  Get specific locale.
        /// </summary>
        /// <param name="locale">culture name.</param>
        /// <returns>locale.</returns>
        ILocale GetLocale(string locale);

        /// <summary>
        ///     Change current locale. This method reload localization dictionary.
        /// </summary>
        /// <param name="locale">locale.</param>
        /// <param name="progress">progress changed.</param>
        /// <returns>task.</returns>
        Task ChangeLocale(ILocale locale, IProgress<float> progress = null);

        /// <summary>
        ///     Change current locale. This method reload localization dictionary.
        /// </summary>
        /// <param name="locale">locale.</param>
        /// <param name="progress">progress changed.</param>
        /// <returns>task.</returns>
        Task ChangeLocale(string locale, IProgress<float> progress = null);
    }
}