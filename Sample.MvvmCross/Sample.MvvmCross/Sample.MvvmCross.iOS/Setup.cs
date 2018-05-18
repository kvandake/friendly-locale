using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform.Platform;
using UIKit;

namespace Sample.MvvmCross.iOS
{
	using System.Globalization;
	using global::MvvmCross.Localization;
	using global::MvvmCross.Platform;
	using global::MvvmCross.Platform.Converters;

	public class Setup : MvxIosSetup
	{
		private Core.App app = new Core.App();
		
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
			: base(applicationDelegate, window)
		{
		}

		public Setup(MvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
			: base(applicationDelegate, presenter)
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

		public override void InitializeSecondary()
		{
			base.InitializeSecondary();
			this.app.InitializeCultureInfo(new CultureInfo("en-US"));
		}
	}
}
