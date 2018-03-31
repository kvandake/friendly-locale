namespace FriendlyLocale.Tests.Units
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Impl;
    using FriendlyLocale.Models;
    using NUnit.Framework;

    [TestFixture]
    public class RemoteTranslateContentClientTests
    {
        [SetUp]
        public void SetUp()
        {
            var remoteConfig = new RemoteContentConfig
            {
                CacheDir = Path.GetTempPath()
            };
            this.remoteTranslateContentClient = new RemoteTranslateContentClient(
                new PlatformComponentsFactory(),
                remoteConfig);
            this.remoteLocale = new RemoteLocale("ru", TestLocaleUrl);
        }

        public const string TestLocaleUrl =
            "https://docs.google.com/uc?export=download&id=1ssSiPncSkonuA8XTvugPNdRoPFsoPHhR";

        private RemoteTranslateContentClient remoteTranslateContentClient;
        private RemoteLocale remoteLocale;

        [Test]
        public async Task Chech_RemoteLanguageStatus()
        {
            // Arrange & Act
            Assert.IsFalse(this.remoteLocale.Downloaded);
            var content = await this.remoteTranslateContentClient.GetContent(this.remoteLocale, null);

            // Assert
            Assert.IsNotEmpty(content);
            Assert.IsTrue(this.remoteLocale.Downloaded);
        }

        [Test]
        public async Task Check_GetCachedContent()
        {
            // Arrange & Act
            Assert.IsFalse(this.remoteLocale.Downloaded);
            var content = await this.remoteTranslateContentClient.GetContent(this.remoteLocale, null);
            Assert.IsTrue(this.remoteLocale.Downloaded);
            var cachedContent = await this.remoteTranslateContentClient.GetContent(this.remoteLocale, null);

            // Assert
            Assert.AreEqual(content, cachedContent);
        }

        [Test]
        public async Task Crash_Download_File()
        {
            // Arrange
            var badRemoteLanguage = new RemoteLocale("ru", "badbad");

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(
                () => this.remoteTranslateContentClient.GetContent(badRemoteLanguage, null));
        }

        [Test]
        public async Task Download_File()
        {
            // Arrange & Act
            var content = await this.remoteTranslateContentClient.GetContent(this.remoteLocale, null, new CancellationToken());

            // Assert
            Assert.IsNotEmpty(content);
        }

        [Test]
        public async Task Download_File_WithProgress()
        {
            // Arrange & Act
            var progressList = new List<float>();
            var progressReporter = new Progress<float>();
            progressReporter.ProgressChanged += (s, e) =>
            {
                lock (this)
                {
                    progressList.Add(e);
                }
            };
            var content = await this.remoteTranslateContentClient.GetContent(this.remoteLocale, progressReporter, new CancellationToken());

            // Assert
            Assert.IsNotEmpty(content);
            Assert.IsTrue(progressList.Count > 0);
            Assert.Greater(progressList.First(), 0);
            Assert.LessOrEqual(progressList.Last(), 100);
        }

        [Test]
        public void GetLanguages()
        {
            // Arrange
            var remoteConfig = new RemoteContentConfig
            {
                Locales =
                {
                    ["ru"] = TestLocaleUrl
                }
            };
            this.remoteTranslateContentClient = new RemoteTranslateContentClient(
                new PlatformComponentsFactory(),
                remoteConfig);

            // Act
            var locales = this.remoteTranslateContentClient.GetLocales();

            // Assert
            Assert.AreEqual(1, locales.Count);
        }
    }
}