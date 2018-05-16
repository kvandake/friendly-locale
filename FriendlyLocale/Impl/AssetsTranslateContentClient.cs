namespace FriendlyLocale.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Interfaces;

    internal class AssetsTranslateContentClient : ITranslateContentClient
    {
        private readonly AssetsContentConfig contentConfig;
        private readonly IPlatformResourceFileManager platformResourceFileManager;

        public AssetsTranslateContentClient(IPlatformComponentsFactory platformComponentsFactory, AssetsContentConfig contentConfig)
        {
            this.platformResourceFileManager = platformComponentsFactory.CreateResourceFileManager();
            this.contentConfig = contentConfig;
        }
        
        public IContentConfig ContentConfig => this.contentConfig;

        public IList<ILocale> GetLocales()
        {
            var supportedResources = this.platformResourceFileManager.FindFileNames(this.contentConfig.ResourceFolder);
            supportedResources = supportedResources.Where(name => name.EndsWith(I18NProvider.YamlFileExtension))
                .Select(Path.GetFileName)
                .ToList();

            return Utils.ConvertFilesToLocales(supportedResources);
        }

        public async Task<string[]> GetContent(ILocale locale, IProgress<float> progress, CancellationToken ct)
        {
            progress?.Report(0);
            var filePath = Utils.GetFilePath(this.contentConfig.ResourceFolder, locale.Source);
            var result = await this.platformResourceFileManager.GetFile(filePath);
            progress?.Report(100);
            return new[] {result};
        }
    }
}