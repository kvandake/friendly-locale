namespace FriendlyLocale.Sample
{
    using System.Reflection;
    using FriendlyLocale.Configs;

    public class AssemblyTranslateViewModel : BaseTranslateViewModel
    {
        public AssemblyTranslateViewModel()
        {
            this.Title = "Assembly";
        }

        protected override void ReloadLocale()
        {
            var assemblyConfig = new AssemblyContentConfig(this.GetType().GetTypeInfo().Assembly)
            {
                ResourceFolder = "Locales"
            };

            FriendlyLocale.I18N.Initialize(assemblyConfig);
            this.ReloadItems();
        }
    }
}