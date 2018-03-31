namespace FriendlyLocale.Configs
{
    using System.Reflection;

    public class AssemblyContentConfig
    {
        public AssemblyContentConfig(Assembly hostAssembly)
        {
            this.HostAssembly = hostAssembly;
        }

        public Assembly HostAssembly { get; }

        public string ResourceFolder { get; set; }
    }
}