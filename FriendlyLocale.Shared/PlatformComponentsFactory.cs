namespace FriendlyLocale
{
    using System;
    using FriendlyLocale.Interfaces;

    public class PlatformComponentsFactory : IPlatformComponentsFactory
    {
        public IPlatformCacheFileManager CreateCacheFileManager()
        {
#if PCL
            throw new ArgumentException("This is the PCL library, not the platform library. " +
                                        " You must install the nuget package in your main executable/application project");
#else
            return new PlatformCacheFileManager();
#endif
        }

        public IPlatformHttpClientManager CreateHttpClientManager()
        {
#if PCL
            throw new ArgumentException("This is the PCL library, not the platform library. " +
                                        " You must install the nuget package in your main executable/application project");
#else
            return new PlatformHttpClientManager();
#endif
        }

        public IPlatformResourceFileManager CreateResourceFileManager()
        {
#if PCL
            throw new ArgumentException("This is the PCL library, not the platform library.  " +
                                        "You must install the nuget package in your main executable/application project");
#else
            return new PlatformResourceFileManager();
#endif
        }
    }
}