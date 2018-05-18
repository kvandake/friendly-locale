using Android.Content;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Platform.Platform;
using System.Globalization;
using Acr.UserDialogs;
using global::MvvmCross.Binding;
using global::MvvmCross.Binding.Parse.Binding.Lang;
using global::MvvmCross.Localization;
using global::MvvmCross.Platform;
using global::MvvmCross.Platform.Converters;
using global::MvvmCross.Platform.Droid.Platform;

namespace Sample.MvvmCross.Droid
{
	public class Setup : MvxAppCompatSetup
	{
		private Core.App app = new Core.App();
		
		public Setup(Context applicationContext) : base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return app;
		}

		protected override IMvxTrace CreateDebugTrace()
		{
			return new DebugTrace();
		}

		protected override void FillValueConverters(IMvxValueConverterRegistry registry)
		{
			base.FillValueConverters(registry);
			registry.AddOrOverwrite("Language", new MvxLanguageConverter());
		}

		public override void InitializePrimary()
		{
			base.InitializePrimary();
			UserDialogs.Init(() => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity);
			Mvx.CallbackWhenRegistered<IMvxLanguageBindingParser>(parser =>
			{
				var parserIntance = (MvxLanguageBindingParser) parser;
				parserIntance.DefaultTextSourceName ="LocalizedTextSource";
				parserIntance.DefaultBindingMode = MvxBindingMode.OneWay;
			});
		}

		public override void InitializeSecondary()
		{
			base.InitializeSecondary();
			this.app.InitializeCultureInfo(new CultureInfo("en-US"));
		}
	}
}
