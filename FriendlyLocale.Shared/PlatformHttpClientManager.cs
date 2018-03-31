#if __ANDROID__ || __IOS__ || __TEST__
namespace FriendlyLocale
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using FriendlyLocale.Interfaces;

    public class PlatformHttpClientManager : IPlatformHttpClientManager
    {
        public Func<HttpClientHandler> HttpClientHandlerFunc { get; set; }

        public async Task<string> DownloadContent(string url, IProgress<float> progress, CancellationToken ct)
        {
            using (var client = this.HttpClientHandlerFunc != null
                ? new HttpClient(this.HttpClientHandlerFunc())
                : new HttpClient())
            {
                using (var stream = await client.GetStreamAsync(url))
                {
                    var buffer = new byte[128];
                    var totalBytes = stream.Length;
                    long receivedBytes = 0;
                    var receivedText = string.Empty;

                    for (;;)
                    {
                        var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, ct);
                        if (bytesRead == 0)
                        {
                            await Task.Yield();
                            break;
                        }

                        var text = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        receivedBytes += bytesRead;
                        receivedText += text;

                        var progressPercentage = totalBytes < 0 ? 0 : 100 * receivedBytes / totalBytes;
                        progress?.Report(progressPercentage);
                    }

                    return receivedText;
                }
            }
        }
    }
}

#endif