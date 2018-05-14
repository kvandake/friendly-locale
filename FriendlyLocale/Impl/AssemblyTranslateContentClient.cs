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
    using FriendlyLocale.Models;

    internal class AssemblyTranslateContentClient : ITranslateContentClient
    {
        private readonly AssemblyContentConfig contentConfig;

        public AssemblyTranslateContentClient(AssemblyContentConfig contentConfig)
        {
            this.contentConfig = contentConfig;
        }

        public IList<ILocale> GetLocales()
        {
            var locales = new List<AssemblyLocale>();
            foreach (var hostAssembly in this.contentConfig.HostAssemblies)
            {
                var localeResources = hostAssembly
                    .GetManifestResourceNames()
                    .Where(x => x.Contains($".{this.contentConfig.ResourceFolder}."));
                var supportedResources = localeResources.Where(name => name.EndsWith(I18NProvider.YamlFileExtension)).ToList();
                foreach (var supportedResource in supportedResources)
                {
                    var localeName = Utils.GetLocaleFromFile(supportedResource);
                    // берем название файла с расширением
                    var localeSource = string.Concat(localeName, Path.GetExtension(supportedResource));
                    var existsLocale = locales.FirstOrDefault(x => x.Key == localeName);
                    if (existsLocale == null)
                    {
                        locales.Add(new AssemblyLocale(new List<Assembly> {hostAssembly}, localeName, localeSource));
                    }
                    else
                    {
                        existsLocale.HostAssemblies.Add(hostAssembly);
                    }
                }
            }

            return locales.ToList<ILocale>();
        }

        public async Task<string[]> GetContent(ILocale locale, IProgress<float> progress, CancellationToken ct)
        {
            if (!(locale is AssemblyLocale assemblyLocale))
            {
                throw new NotSupportedException($"Unsupported type: {locale}. Please use AssemblyLocale for AssemblyContent");
            }

            progress?.Report(0);
            var contents = new List<string>();
            foreach (var hostAssembly in assemblyLocale.HostAssemblies)
            {
                contents.Add(await GetAssemblyContent(
                    hostAssembly,
                    assemblyLocale.GetSourcePath(hostAssembly, this.contentConfig.ResourceFolder)));
            }

            progress?.Report(100);
            return contents.ToArray();
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