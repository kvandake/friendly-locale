﻿namespace FriendlyLocale.Models
{
    using System.Collections.Generic;
    using System.Reflection;

    internal class AssemblyLocale : Locale
    {
        public AssemblyLocale(IList<Assembly> hostAssemblies, string key, string source)
            : base(key, source)
        {
            this.HostAssemblies = hostAssemblies;
        }

        public IList<Assembly> HostAssemblies { get; }

        public string GetSourcePath(Assembly hostAssembly, string folderName)
        {
            return $"{hostAssembly.GetName().Name}.{folderName}.{this.Source}";
        }
    }
}