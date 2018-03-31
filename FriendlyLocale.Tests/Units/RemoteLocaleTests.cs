namespace FriendlyLocale.Tests.Units
{
    using System.Collections.Generic;
    using FriendlyLocale.Models;
    using NUnit.Framework;

    [TestFixture]
    public class RemoteLocaleTests
    {
        [Test]
        public void Check_Unique_CacheSource()
        {
               // Arrange
            var urls = new List<string>
            {
                "12345",
                "12344",
                "12345.yaml",
                "1234.yaml",
                "1.yaml",
                "12.yaml",
                "test.yaml",
                "tset.yaml",
                "https://google.com/ru.yaml",
                "https://google.com/en.yaml",
                "https://google.com/kz.yaml",
            };
            
            var locales = new Dictionary<string, string>();
            
            // Act & Assert
            foreach (var url in urls)
            {
                var language = new RemoteLocale("ru", url);
                locales.Add(language.CacheSource, language.Key);
            }
        }
    }
}