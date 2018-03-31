namespace FriendlyLocale.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Exceptions;
    using FriendlyLocale.Interfaces;
    using FriendlyLocale.Models;

    internal class OfflineRemoteTranslateContentClient : RemoteTranslateContentClient
    {
        private readonly OfflineContentConfig offlineContentConfig;
        private IPlatformResourceFileManager platformResourceFileManager;

        public OfflineRemoteTranslateContentClient(
            IPlatformComponentsFactory platformComponentsFactory,
            RemoteContentConfig contentConfig,
            OfflineContentConfig offlineContentConfig)
            : base(platformComponentsFactory, contentConfig)
        {
            this.offlineContentConfig = offlineContentConfig;
        }

        public IPlatformResourceFileManager PlatformResourceFileManager =>
            this.platformResourceFileManager ??
            (this.platformResourceFileManager = this.PlatformComponentsFactory.CreateResourceFileManager());

        private Task<string> GetOfflineContent()
        {
            var resourceFolder = this.offlineContentConfig.ResourceFolder;
            var fileName = this.offlineContentConfig.FileName;
            if (this.offlineContentConfig.IsLocal)
            {
                var filePath = Utils.GetFilePath(resourceFolder, fileName);
                return this.PlatformResourceFileManager.GetFile(filePath);
            }

            var assemblyFilePath = string.IsNullOrEmpty(resourceFolder) ? fileName : $".{resourceFolder}.{fileName}";
            var assembly = this.offlineContentConfig.Assembly;
            var assemblyResource = assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains(assemblyFilePath));
            if (string.IsNullOrEmpty(assemblyResource))
            {
                throw new FriendlyTranslateException();
            }
            
            return AssemblyTranslateContentClient.GetAssemblyContent(assembly, assemblyResource);
        }

        public override Task<string> GetContent(ILocale locale, IProgress<float> progressAction,
            CancellationToken ct = default(CancellationToken))
        {
            return locale is OfflineLocale ? this.GetOfflineContent() : base.GetContent(locale, progressAction, ct);
        }

        public override IList<ILocale> GetLocales()
        {
            var locales = base.GetLocales();
            locales.Insert(0, new OfflineLocale(this.offlineContentConfig));
            return locales;
        }
    }
}