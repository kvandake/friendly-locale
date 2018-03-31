#if __ANDROID__ || __IOS__
namespace FriendlyLocale
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using FriendlyLocale.Interfaces;

    public class PlatformCacheFileManager : IPlatformCacheFileManager
    {
        private string personalFolder;

        public bool ContainsFile(string filePath)
        {
            return File.Exists(this.WrapPlatform(filePath));
        }

        public IList<string> FindFileNames(string dir)
        {
            return Directory.GetFiles(this.WrapPlatform(dir));
        }

        public async Task UpsertFile(string filePath, string content)
        {
            File.WriteAllText(this.WrapPlatform(filePath), content);
        }

        public Task<string> GetFile(string filePath)
        {
            return Task.FromResult(File.ReadAllText(this.WrapPlatform(filePath)));
        }

        private string WrapPlatform(string filePath)
        {
            return string.IsNullOrEmpty(filePath) ? filePath : Path.Combine(this.GetPersonalFolder(), filePath);
        }

        private string GetPersonalFolder()
        {
            return this.personalFolder ??
                   (this.personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal));
        }
    }
}
#endif

#if __TEST__
namespace FriendlyLocale
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using global::FriendlyLocale.Interfaces;

    public class PlatformCacheFileManager : IPlatformCacheFileManager
    {
        public bool ContainsFile(string filePath)
        {
            return File.Exists(filePath);
        }

        public IList<string> FindFileNames(string dir)
        {
            return Directory.GetFiles(dir);
        }

        public async Task UpsertFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
        }

        public Task<string> GetFile(string filePath)
        {
            return Task.FromResult(File.ReadAllText(filePath));
        }
    }
}
#endif