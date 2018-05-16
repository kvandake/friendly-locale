namespace FriendlyLocale.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FriendlyLocale.Exceptions;
    using FriendlyLocale.Extensions;
    using FriendlyLocale.Interfaces;
    using FriendlyLocale.Parser;

    public class I18NProvider : II18N
    {
        public static string YamlFileExtension = "yaml";

        public I18NProvider(ITranslateContentClient contentClient)
        {
            this.ContentClient = contentClient;
            this.Locales = this.ContentClient.GetLocales();
        }

        private IList<ILocale> Locales { get; }

        private ITranslateContentClient ContentClient { get; }

        private bool HasLogger => this.ContentClient.ContentConfig.Logger != null;

        internal YParser Parser { get; set; }

        public string FallbackLocale { get; set; }

        public string Translate(string[] innerKeys, string fallback)
        {
            return this.Parser?.FindValue(innerKeys) ?? fallback;
        }

        public string Translate(string key, string fallback)
        {
            return this.Parser?.FindValue(key) ?? fallback;
        }

        public string Translate(string key, params object[] args)
        {
            var content = this.Parser?.FindValue(key);
            return string.IsNullOrWhiteSpace(content) ? string.Empty : string.Format(content, args);
        }

        public string TranslateNamingFormat(string key, object injectionObject)
        {
            var content = this.Parser?.FindValue(key);
            return content?.InjectNamedFormats(injectionObject);
        }

        public string TranslateEnum<TEnum>(TEnum enumValue, string fallback)
        {
            var type = typeof(TEnum);
            var enumName = Enum.GetName(type, enumValue);
            return this.Translate($"{type.Name}.{enumName}", fallback);
        }

        public ILocale CurrentLocale { get; private set; }

        public IEnumerable<ILocale> GetAvailableLocales()
        {
            return this.Locales;
        }

        public ILocale GetLocale(string locale)
        {
            return this.Locales?.FirstOrDefault(x => x.Key == locale);
        }

        public async Task ChangeLocale(ILocale locale, IProgress<float> progress)
        {
            try
            {
                if (locale == null)
                {
                    throw FriendlyTranslateException.NotFoundLocale();
                }

                var content = await this.ContentClient.GetContent(locale, progress).ConfigureAwait(false);
                this.Parser = new YParser(content)
                {
                    ThrowWhenKeyNotFound = this.ContentClient.ContentConfig.ThrowWhenKeyNotFound
                };
                
                this.CurrentLocale = locale;
                this.LogTranslations();
            }
            catch (Exception)
            {
                if (locale?.Key == this.FallbackLocale)
                {
                    throw;
                }

                var fallbackLocale = this.GetLocale(this.FallbackLocale);
                if (fallbackLocale == null)
                {
                    throw;
                }

                await this.ChangeLocale(fallbackLocale, progress);
            }
        }

        public Task ChangeLocale(string locale, IProgress<float> progress)
        {
            return this.ChangeLocale(this.GetLocale(locale), progress);
        }

        private void LogTranslations()
        {
            if (!this.HasLogger)
            {
                return;
            }

            this.Log("========== I18N translations ==========");
            foreach (var item in this.Parser.map)
            {
                this.Log($"{item.Key} = {item.Value}");
            }

            this.Log("====== I18N end of translations =======");
        }

        private void Log(string trace)
        {
            this.ContentClient.ContentConfig.Logger?.Invoke(trace);
        }
    }
}