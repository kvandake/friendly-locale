using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using System.Reflection;
using Acr.UserDialogs;
using FriendlyLocale.Configs;
using global::MvvmCross.Localization;
using global::MvvmCross.Platform;
using global::MvvmCross.Platform.Logging;
using YamlLocalization;

namespace Sample.MvvmCross.Core
{
	using System.Globalization;

	public class App : MvxApplication
	{
		private static IMvxLog localeLog;

		private static IMvxLog LocaleLog => localeLog ?? (localeLog = CreateMvxLog());

		private static IMvxLog CreateMvxLog()
		{
			return Mvx.Resolve<IMvxLogProvider>().GetLogFor("MvxLocale");
		}
		
		public override void Initialize()
		{
			CreatableTypes()
				.EndingWith("Service")
				.AsInterfaces()
				.RegisterAsLazySingleton();

			RegisterNavigationServiceAppStart<ViewModels.FirstViewModel>();

			Mvx.RegisterSingleton(UserDialogs.Instance);
			var assemblyConfig = new AssemblyContentConfig(this.GetType().GetTypeInfo().Assembly)
			{
				ResourceFolder = "Locales",
				ThrowWhenKeyNotFound = true,
				Logger = trace => { 
					LocaleLog.Debug(trace); 
				}
			};

			var textProvider = new MvxYamlTextProvider(assemblyConfig);

			Mvx.RegisterSingleton<IMvxTextProvider>(textProvider);
			Mvx.RegisterSingleton<IMvxLocalizationProvider>(textProvider);
		}

		public void InitializeCultureInfo(CultureInfo cultureInfo)
		{
			var localizationProvider = Mvx.Resolve<IMvxLocalizationProvider>();
			localizationProvider.ChangeLocale(cultureInfo).Wait();
		}
	}
}