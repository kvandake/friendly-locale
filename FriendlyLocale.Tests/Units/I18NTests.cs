namespace FriendlyLocale.Tests.Units
{
    using System.IO;
    using System.Threading.Tasks;
    using Configs;
    using NUnit.Framework;

    [TestFixture]
    public class I18NTests
    {
        [Test]
        public async Task Chech_OfflineLocale()
        {
            // Arrange
            var hostAssembly = this.GetType().Assembly;
            var offlineConfig = OfflineContentConfig.FromAssembly(hostAssembly, "en.yaml", "Locales");
            var remoteConfig = new RemoteContentConfig
            {
                CacheDir = Path.GetTempPath()
            };

            I18N.Initialize(remoteConfig, offlineConfig);

            // Act
            var offlineLocale = I18N.Instance.GetLocale("offline");
            await I18N.Instance.ChangeLocale(offlineLocale);
            var value = I18N.Instance.Translate("ViewModel.Locale");

            // Assert
            Assert.NotNull(offlineLocale);
            Assert.AreEqual("en", value);
        }

        [Test]
        public async Task Translate_SomeWords()
        {
            // Arrange

            I18N.Initialize(new AssemblyContentConfig(this.GetType().Assembly)
            {
                ResourceFolder = "Locales"
            });

            var friendlyLocale = I18N.Instance;

            await friendlyLocale.ChangeLocale("en");

            // Act
            var value = friendlyLocale.Translate("ViewModel.Locale");

            // Assert
            Assert.AreEqual("en", value);
        }

        [Test]
        public async Task Translate_SomeWordsWithChangeLocale()
        {
            // Arrange
            I18N.Initialize(new AssemblyContentConfig(this.GetType().Assembly)
            {
                ResourceFolder = "Locales"
            });

            var friendlyLocale = I18N.Instance;

            await friendlyLocale.ChangeLocale("en");

            // Act
            var value = friendlyLocale.Translate("ViewModel.Locale");
            Assert.AreEqual("en", value);
            await friendlyLocale.ChangeLocale("ru");
            value = friendlyLocale.Translate("ViewModel.Locale");

            // Assert
            Assert.AreEqual("ru", value);
        }

        [Test]
        public async Task Translate_SomeWordsWithFallback()
        {
            // Arrange
            I18N.Initialize(new AssemblyContentConfig(this.GetType().Assembly)
            {
                ResourceFolder = "Locales"
            });

            var friendlyLocale = I18N.Instance;
            friendlyLocale.FallbackLocale = "en";

            await friendlyLocale.ChangeLocale("fr");

            // Act
            var value = friendlyLocale.Translate("ViewModel.Locale");

            // Assert
            Assert.AreEqual("en", value);
        }
    }
}