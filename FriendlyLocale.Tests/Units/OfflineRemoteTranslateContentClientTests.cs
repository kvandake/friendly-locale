namespace FriendlyLocale.Tests.Units
{
    using System.IO;
    using System.Threading.Tasks;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Impl;
    using FriendlyLocale.Models;
    using NUnit.Framework;

    [TestFixture]
    public class OfflineRemoteTranslateContentClientTests
    {
        private OfflineRemoteTranslateContentClient offlineRemoteTranslateContentClient;
        private OfflineContentConfig offlineContentConfig;

        [SetUp]
        public void SetUp()
        {
            var remoteConfig = new RemoteContentConfig
            {
                CacheDir = Path.GetTempPath()
            };

            var hostAssembly = this.GetType().Assembly;
            this.offlineContentConfig = OfflineContentConfig.FromAssembly(hostAssembly, "en.yaml", "Locales");

            this.offlineRemoteTranslateContentClient = new OfflineRemoteTranslateContentClient(
                new PlatformComponentsFactory(),
                remoteConfig,
                this.offlineContentConfig);
        }

        [Test]
        public async Task Chech_OfflineLocale()
        {
            // Arrange
            var offlineLocale = new OfflineLocale(this.offlineContentConfig);
            
            // Act
            var content = await this.offlineRemoteTranslateContentClient.GetContent(offlineLocale, null);

            // Assert
            Assert.IsNotEmpty(content);
        }
    }
}