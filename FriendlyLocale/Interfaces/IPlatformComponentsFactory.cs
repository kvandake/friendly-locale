namespace FriendlyLocale.Interfaces
{
    public interface IPlatformComponentsFactory
    {
        IPlatformCacheFileManager CreateCacheFileManager();

        IPlatformHttpClientManager CreateHttpClientManager();

        IPlatformResourceFileManager CreateResourceFileManager();
    }
}