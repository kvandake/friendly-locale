# Friendly Locale
Simple and cross platform internationalization for Xamarin and .NET. The localization is similar to [Ruby On Rails](http://guides.rubyonrails.org/i18n.html).
[![Build Status](https://travis-ci.org/kvandake/friendly-locale.svg?branch=master)](https://travis-ci.org/kvandake/friendly-locale)

Features:
- Cross platform;
- No dependencies;
- Support remote resources;
- Support assets resources;
- Support embedded resources.

## Embedded resources from Assembly
```cs
var assembly = this.GetType().GetTypeInfo().Assembly;
var assemblyConfig = new AssemblyContentConfig(assembly)
{
    ResourceFolder = "Locales"
};

FriendlyLocale.I18N.Initialize(assemblyConfig);
```

## Assets resources
```cs
var assetsConfig = new LocalContentConfig
{
    ResourceFolder = "Locales"
};

FriendlyLocale.I18N.Initialize(assetsConfig);
```

## Remote resources
### Supported an offline mode
- #### Assets file
```cs
var offlineConfig = OfflineContentConfig.FromLocal("en.yaml", "Locales");
```
- #### Embedded resource
```cs
var assembly = this.GetType().GetTypeInfo().Assembly;
var offlineConfig = OfflineContentConfig.FromAssembly(assembly, "ru.yaml", "Locales");
```
### Setup
```cs
var remoteConfig = new RemoteContentConfig
{
    Locales =
    {
        {"ru", "https://any.com/ru.yaml"},
        {"en", "https://any.com/en.yaml"}
    }
};
// offlineConfig - optional
FriendlyLocale.I18N.Initialize(remoteConfig, offlineConfig);
```
