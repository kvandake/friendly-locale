namespace FriendlyLocale.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Interfaces;

    internal class AssemblyTranslateContentClient : ITranslateContentClient
    {
        private readonly AssemblyContentConfig contentConfig;

        public AssemblyTranslateContentClient(AssemblyContentConfig contentConfig)
        {
            this.contentConfig = contentConfig;
        }

        public IList<ILocale> GetLocales()
        {
            var localeResources = this.contentConfig.HostAssembly
                .GetManifestResourceNames()
                .Where(x => x.Contains($".{this.contentConfig.ResourceFolder}."));

            var supportedResources =
                localeResources.Where(name => name.EndsWith(I18NProvider.YamlFileExtension)).ToList();

            return Utils.ConvertFilesToLocales(supportedResources);
        }

        public async Task<string> GetContent(ILocale locale, IProgress<float> progress, CancellationToken ct)
        {
            progress?.Report(0);
            var result = await GetAssemblyContent(this.contentConfig.HostAssembly, locale.Source);
            progress?.Report(100);
            return result;
        }

        public static Task<string> GetAssemblyContent(Assembly assembly, string source)
        {
            using (var stream = assembly.GetManifestResourceStream(source))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEndAsync();
                }
            }
        }
    }
}