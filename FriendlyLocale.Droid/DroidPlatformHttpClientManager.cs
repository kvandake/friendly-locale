namespace FriendlyLocale
{
    using System;
    using System.Threading.Tasks;
    using FriendlyLocale.Interfaces;

    public class DroidPlatformHttpClientManager : IPlatformHttpClientManager
    {
        public Task<string> DownloadContent(string url, Action<int> progressAction)
        {
            throw new NotImplementedException();
        }
    }
}