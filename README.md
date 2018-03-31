# Friendly Locale(Beta)
- https://kvandake.github.io/friendly-locale/
---
Simple and cross platform internationalization for Xamarin and .NET. The localization is similar to [Ruby On Rails](http://guides.rubyonrails.org/i18n.html).
[![Build Status](https://travis-ci.org/kvandake/friendly-locale.svg?branch=master)](https://travis-ci.org/kvandake/friendly-locale)

Features:
- Cross platform;
- No dependencies;
- Support **Remote Resources**;
- Support **Assets Resources**;
- Support **Embedded Resources**.

## Install
https://www.nuget.org/packages/FriendlyLocale
```
PM> Install-Package FriendlyLocale
```

To use, simply reference the nuget package in each of your platform projects.
* **You can not add to each platform if you use embedded resources from the assembly.**

## Setup

### Embedded resources from Assembly
[AssemblyContentConfig](FriendlyLocale/Configs/AssemblyContentConfig.cs)
```cs
var assembly = this.GetType().GetTypeInfo().Assembly;
var assemblyConfig = new AssemblyContentConfig(assembly)
{
    ResourceFolder = "Locales"
};

FriendlyLocale.I18N.Initialize(assemblyConfig);
```

### Assets resources
[AssetsContentConfig](FriendlyLocale/Configs/AssetsContentConfig.cs)
```cs
var assetsConfig = new LocalContentConfig
{
    ResourceFolder = "Locales"
};

FriendlyLocale.I18N.Initialize(assetsConfig);
```

### Remote resources
[RemoteContentConfig](FriendlyLocale/Configs/RemoteContentConfig.cs)
#### Offline mode is supported
[OfflineContentConfig](FriendlyLocale/Configs/OfflineContentConfig.cs)
- ##### Assets file
```cs
var offlineConfig = OfflineContentConfig.FromAssets("en.yaml", "Locales");
```
- ##### Embedded resource
```cs
var assembly = this.GetType().GetTypeInfo().Assembly;
var offlineConfig = OfflineContentConfig.FromAssembly(assembly, "ru.yaml", "Locales");
```
And the final step of initialization
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

## Localization file syntax
More inforamtion on http://www.yaml.org/spec/1.2/spec.html.
### Simple using
```yaml
FirstViewModel:
  Title: "Title"
  Empty:
    Title: "Not found"
    TitleButton: "Refresh data"
```
Get the value:
```cs
var title = I18N.Instance.Translate("FirstViewModel.Title");
var emptyTitle = I18N.Instance.Translate("FirstViewModel.Empty.Title");
var emptyTitleButton = I18N.Instance.Translate("FirstViewModel.Empty.TitleButton");
```

### Using Multiline
```yaml
FirstViewModel:
  MultiDescription1: |
    MultiDescription: Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
    Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, 
  MultiDescription2: >
    Lorem Ipsum is simply dummy text of the printing and typesetting industry. 
    Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, 
```
`>` - [Folded style](http://www.yaml.org/spec/1.2/spec.html#id2796251) removes single newlines within the string (but adds one at the end, and converts double newlines to singles).

`|` - [Literal style](http://www.yaml.org/spec/1.2/spec.html#id2795688) turns every newline within the string into a literal newline, and adds one at the end.

Get the value:
```cs
var multiDescription1 = I18N.Instance.Translate("FirstViewModel.MultiDescription1");
var multiDescription2 = I18N.Instance.Translate("FirstViewModel.MultiDescription2");
```

### Using Anchor - Alias
More inforamtion on https://learnxinyminutes.com/docs/yaml/.
```yaml
alias: &ALIAS
  AliasDescription: "AliasDescription"
  
FirstViewModel:
  <<: *ALIAS
```
Get the value:
```cs
var aliasDescription = I18N.Instance.Translate("FirstViewModel.AliasDescription");
```

## Sample
[Sample project](FriendlyLocale.Sample)
![screen](docs/screen.png)

## Roadmap
- Add more tests;
- Automatic change of values when changing localization;
- Detailed code documentation.
