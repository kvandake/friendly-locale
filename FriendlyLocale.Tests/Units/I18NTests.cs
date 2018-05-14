﻿namespace FriendlyLocale.Tests.Units
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using Configs;
    using FriendlyLocale.Impl;
    using FriendlyLocale.Parser;
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
        
        [Test]
        public async Task Translate_SomeWordsWithArgs()
        {
            // Arrange
            I18N.Initialize(new AssemblyContentConfig(this.GetType().Assembly)
            {
                ResourceFolder = "Locales"
            });

            var friendlyLocale = I18N.Instance;
            await friendlyLocale.ChangeLocale("en");

            // Act
            var value = friendlyLocale.Translate("ViewModel.Test1.Test2.TestArgs", 5);

            // Assert
            Assert.AreEqual("Any 5", value);
        }
        
        [Test]
        public async Task Translate_SomeWordsWithArgs_WithoutArgs()
        {
            // Arrange
            I18N.Initialize(new AssemblyContentConfig(this.GetType().Assembly)
            {
                ResourceFolder = "Locales"
            });

            var friendlyLocale = I18N.Instance;
            await friendlyLocale.ChangeLocale("en");

            // Act
            var value = friendlyLocale.Translate("ViewModel.Locale", 5);

            // Assert
            Assert.AreEqual("en", value);
        }
        
        [Test]
        public async Task Translate_MultipleAssemblies()
        {
            // Arrange
            var hostAssembly = this.GetType().Assembly;
            I18N.Initialize(new AssemblyContentConfig(new List<Assembly> {hostAssembly, hostAssembly})
            {
                ResourceFolder = "Locales"
            });

            var friendlyLocale = I18N.Instance;
            await friendlyLocale.ChangeLocale("en");

            // Act
            var value = friendlyLocale.Translate("ViewModel.Locale");

            // Assert
            Assert.AreEqual(7, (friendlyLocale as I18NProvider)?.Parser?.map.Count);
            Assert.AreEqual("en", value);
        }
    }
}