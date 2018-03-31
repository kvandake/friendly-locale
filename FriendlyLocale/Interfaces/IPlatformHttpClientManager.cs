namespace FriendlyLocale.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPlatformHttpClientManager
    {
        Task<string> DownloadContent(string url, IProgress<float> progress,
            CancellationToken ct = default(CancellationToken));
    }
}