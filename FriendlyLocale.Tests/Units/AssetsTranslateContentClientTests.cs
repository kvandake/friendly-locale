namespace FriendlyLocale.Tests.Units
{
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FriendlyLocale.Models;
    using global::FriendlyLocale.Configs;
    using global::FriendlyLocale.Impl;
    using NUnit.Framework;

    [TestFixture]
    public class AssetsTranslateContentClientTests
    {
        private AssetsTranslateContentClient assetsTranslateContentClient;

        [Test]
        public async Task Check_DownloadContent()
        {
            // Arrange
            var remoteConfig = new RemoteContentConfig
            {
                CacheDir = Path.GetTempPath()
            };

            var localConfig = new AssetsContentConfig
            {
                ResourceFolder = remoteConfig.CacheDir
            };

            if (!Directory.Exists(localConfig.ResourceFolder))
            {
                Directory.CreateDirectory(localConfig.ResourceFolder);
            }

            this.assetsTranslateContentClient =
                new AssetsTranslateContentClient(new PlatformComponentsFactory(), localConfig);
            var remoteTranslateContentClient = new RemoteTranslateContentClient(new PlatformComponentsFactory(), remoteConfig);
            var remoteLocale = new RemoteLocale("ru", RemoteTranslateContentClientTests.TestLocaleUrl);
            await remoteTranslateContentClient.GetContent(remoteLocale, null);
            var locales = this.assetsTranslateContentClient.GetLocales();

            // Act
            var content = await this.assetsTranslateContentClient.GetContent(locales.First(), null, CancellationToken.None);

            // Assert
            Assert.IsNotEmpty(content);
        }
    }
}