#if __ANDROID__ || __IOS__
namespace FriendlyLocale
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using global::FriendlyLocale.Interfaces;

    public class PlatformResourceFileManager : IPlatformResourceFileManager
    {
        public IList<string> FindFileNames(string dir)
        {

#if __ANDROID__
            var assets = Android.App.Application.Context.Assets;
            return assets.List(dir);
#else
            return Directory.GetFiles(dir);
#endif
        }

        public Task<string> GetFile(string filePath)
        {
#if __ANDROID__
            var assets = Android.App.Application.Context.Assets;
            using (StreamReader sr = new StreamReader(assets.Open(filePath)))
            {
                return Task.FromResult(sr.ReadToEnd());
            }
#else
			return Task.FromResult(File.ReadAllText(filePath));
#endif
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
    using FriendlyLocale.Interfaces;

    public class PlatformResourceFileManager : IPlatformResourceFileManager
    {
        public IList<string> FindFileNames(string dir)
        {
            return Directory.GetFiles(dir);
        }

        public Task<string> GetFile(string filePath)
        {
            return Task.FromResult(File.ReadAllText(filePath));
        }
    }
}
#endif