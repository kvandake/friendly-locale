namespace FriendlyLocale
{
    using System;
    using FriendlyLocale.Configs;
    using FriendlyLocale.Impl;
    using FriendlyLocale.Interfaces;

    public static class I18N
    {
        private static Lazy<II18N> instanceLazy;

        public static II18N Instance => instanceLazy.Value;

        private static IPlatformComponentsFactory CreatePlatformComponentsFactory()
        {
            return new PlatformComponentsFactory();
        }

        public static void Initialize(AssemblyContentConfig contentConfig)
        {
            InitializeInternal(TranslateContentClientFactory.Create(contentConfig));
        }

        public static void Initialize(AssetsContentConfig contentConfig)
        {
            InitializeInternal(TranslateContentClientFactory.Create(CreatePlatformComponentsFactory(), contentConfig));
        }

        public static void Initialize(RemoteContentConfig contentConfig)
        {
            InitializeInternal(TranslateContentClientFactory
                .Create(CreatePlatformComponentsFactory(), contentConfig));
        }

        public static void Initialize(RemoteContentConfig contentConfig, OfflineContentConfig offlineContentConfig)
        {
            InitializeInternal(TranslateContentClientFactory
                .Create(CreatePlatformComponentsFactory(), contentConfig, offlineContentConfig));
        }

        public static void Initialize(ITranslateContentClient translateContentClient)
        {
            InitializeInternal(translateContentClient);
        }

        private static void InitializeInternal(ITranslateContentClient translateContentClient)
        {
            instanceLazy = new Lazy<II18N>(() => new I18NProvider(translateContentClient));
        }
    }
}