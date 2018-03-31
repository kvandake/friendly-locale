namespace FriendlyLocale.Configs
{
    using System.Reflection;

    public class OfflineContentConfig
    {
        internal Assembly Assembly { get; set; }

        internal string FileName { get; set; }
        
        internal string ResourceFolder { get; set; }

        internal bool IsLocal { get; private set; }

        public static OfflineContentConfig FromAssembly(Assembly assembly, string fileName, string resourceFolder = null)
        {
            
            return new OfflineContentConfig
            {
                Assembly = assembly,
                FileName = fileName,
                ResourceFolder = resourceFolder,
                IsLocal = false
            };
        }

        public static OfflineContentConfig FromLocal(string fileName, string resourceFolder = null)
        {
            return new OfflineContentConfig
            {
                FileName = fileName,
                ResourceFolder = resourceFolder,
                IsLocal = true
            };
        }
    }
}