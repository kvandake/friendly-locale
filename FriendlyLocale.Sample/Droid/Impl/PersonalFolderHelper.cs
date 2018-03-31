using FriendlyLocale.Sample.Droid.Impl;
using Xamarin.Forms;

[assembly: Dependency(typeof(PersonalFolderHelper))]

namespace FriendlyLocale.Sample.Droid.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using FriendlyLocale.Impl;
    using FriendlyLocale.Sample.Helpers;

    public class PersonalFolderHelper : IPersonalFolderHelper
    {
        public void ClearCache()
        {
            var fileExt = I18NProvider.YamlFileExtension;
            foreach (var file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                $"*.{fileExt}").Where(item => item.EndsWith($".{fileExt}")))
            {
                File.Delete(file);
            }
        }
    }
}