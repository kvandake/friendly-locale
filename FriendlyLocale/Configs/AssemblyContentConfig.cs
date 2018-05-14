namespace FriendlyLocale.Configs
{
    using System.Collections.Generic;
    using System.Reflection;

    public class AssemblyContentConfig
    {
        public AssemblyContentConfig(Assembly hostAssembly)
        {
            this.HostAssemblies = new List<Assembly> {hostAssembly};
        }

        public AssemblyContentConfig(IList<Assembly> hostAssemblies)
        {
            this.HostAssemblies = hostAssemblies;
        }

        public IList<Assembly> HostAssemblies { get; }

        public string ResourceFolder { get; set; }
    }
}