using System.Threading;

namespace FriendlyLocale.Tests.Units
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using FriendlyLocale.Models;
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

        [Test]
        public async Task Check_Multiple_Assemblies()
        {
            // Arrange
            var hostAssembly = this.GetType().Assembly;
            var assemblyConfig = new AssemblyContentConfig(new List<Assembly> {hostAssembly, hostAssembly})
            {
                ResourceFolder = "Locales"
            };
            this.assemblyTranslateContentClient = new AssemblyTranslateContentClient(assemblyConfig);
            var locales = this.assemblyTranslateContentClient.GetLocales();

            // Act
            var contents = await this.assemblyTranslateContentClient.GetContent(locales.First(), null, new CancellationToken());

            // Assert
            Assert.AreEqual(2, contents.Length);
            foreach (var locale in locales)
            {
                Assert.AreEqual(2, (locale as AssemblyLocale)?.HostAssemblies.Count);
            }
        }
    }
}