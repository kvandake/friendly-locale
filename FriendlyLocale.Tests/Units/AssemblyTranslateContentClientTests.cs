using System.Threading;

namespace FriendlyLocale.Tests.Units
{
    using System.Linq;
    using System.Threading.Tasks;
    using global::FriendlyLocale.Configs;
    using global::FriendlyLocale.Impl;
    using NUnit.Framework;

    [TestFixture]
    public class AssemblyTranslateContentClientTests
    {
        private AssemblyTranslateContentClient assemblyTranslateContentClient;

        [SetUp]
        public void SetUp()
        {
            var hostAssembly = this.GetType().Assembly;
            var assemblyConfig = new AssemblyContentConfig(hostAssembly)
            {
                ResourceFolder = "Locales"
            };
            this.assemblyTranslateContentClient = new AssemblyTranslateContentClient(assemblyConfig);
        }

        [Test]
        public async Task Check_DownloadContent()
        {
            // Arrange
            var locales = this.assemblyTranslateContentClient.GetLocales();

            // Act
            var content = await this.assemblyTranslateContentClient.GetContent(locales.First(), null, new CancellationToken());

            // Assert
            Assert.IsNotEmpty(content);
        }
    }
}