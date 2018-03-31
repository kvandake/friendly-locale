namespace FriendlyLocale.Impl
{
    using FriendlyLocale.Configs;
    using FriendlyLocale.Interfaces;

    public static class TranslateContentClientFactory
    {
        public static ITranslateContentClient Create(
            AssemblyContentConfig contentConfig)
        {
            return new AssemblyTranslateContentClient(contentConfig);
        }

        public static ITranslateContentClient Create(
            IPlatformComponentsFactory platformComponentsFactory,
            LocalContentConfig contentConfig)
        {
            return new LocalTranslateContentClient(platformComponentsFactory, contentConfig);
        }

        public static ITranslateContentClient Create(
            IPlatformComponentsFactory platformComponentsFactory,
            RemoteContentConfig contentConfig)
        {
            return new RemoteTranslateContentClient(platformComponentsFactory, contentConfig);
        }

        public static ITranslateContentClient Create(
            IPlatformComponentsFactory platformComponentsFactory,
            RemoteContentConfig contentConfig,
            OfflineContentConfig offlineContentConfig)
        {
            return new OfflineRemoteTranslateContentClient(platformComponentsFactory, contentConfig, offlineContentConfig);
        }
    }
}