namespace FriendlyLocale
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FriendlyLocale.Exceptions;
    using FriendlyLocale.Interfaces;
    using FriendlyLocale.Models;

    internal static class Utils
    {
        public static IList<ILocale> ConvertFilesToLocales(IList<string> files)
        {
            if (files == null || files.Count == 0)
            {
                throw new FriendlyTranslateException("No locales have been found. Make sure you´ve got a folder in the host assembly");
            }

            var result = new Dictionary<string, ILocale>();
            foreach (var resource in files)
            {
                var localeName = GetLocaleFromFile(resource);

                if (result.ContainsKey(localeName))
                {
                    throw new FriendlyTranslateException($"The locales contains a duplicated locale '{localeName}'");
                }

                result[localeName] = new Locale(localeName, resource);
            }

            return result.Values.ToList();
        }

        public static string GetLocaleFromFile(string fileName)
        {
            var parts = fileName.Split('.');
            return parts[parts.Length - 2];
        }

        public static string GetFilePath(string dir, string fileName)
        {
            return string.IsNullOrEmpty(dir) ? fileName : Path.Combine(dir, fileName);
        }
    }
}