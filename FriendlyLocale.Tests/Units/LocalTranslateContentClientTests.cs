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
    public class LocalTranslateContentClientTests
    {
        private LocalTranslateContentClient localTranslateContentClient;

        [Test]
        public async Task Check_DownloadContent()
        {
            // Arrange
            var remoteConfig = new RemoteContentConfig
            {
                CacheDir = Path.GetTempPath()
            };

            var localConfig = new LocalContentConfig
            {
                ResourceFolder = remoteConfig.CacheDir
            };

            if (!Directory.Exists(localConfig.ResourceFolder))
            {
                Directory.CreateDirectory(localConfig.ResourceFolder);
            }

            this.localTranslateContentClient =
                new LocalTranslateContentClient(new PlatformComponentsFactory(), localConfig);
            var remoteTranslateContentClient = new RemoteTranslateContentClient(new PlatformComponentsFactory(), remoteConfig);
            var remoteLocale = new RemoteLocale("ru", RemoteTranslateContentClientTests.TestLocaleUrl);
            await remoteTranslateContentClient.GetContent(remoteLocale, null);
            var locales = this.localTranslateContentClient.GetLocales();

            // Act
            var content = await this.localTranslateContentClient.GetContent(locales.First(), null, CancellationToken.None);

            // Assert
            Assert.IsNotEmpty(content);
        }
    }
}