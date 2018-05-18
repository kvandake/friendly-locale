using System;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using Sample.MvvmCross.Core.ViewModels;

namespace Sample.MvvmCross.iOS.Views
{
	using Sample.MvvmCross.Core.Extensions;

	[MvxFromStoryboard]
	public partial class FirstView : MvxViewController
	{
		public FirstView(IntPtr handle) : base(handle)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			var set = this.CreateBindingSet<FirstView, FirstViewModel>();
			set.Bind(this).For(v => v.Title).ToFlyLocalizationId("Title");
			set.Bind(this.Label).ToFlyLocalizationId("Label");
			set.Bind(this.ChangeLocale).For("Title").ToFlyLocalizationId("Buttons.ChangeLocale");
			set.Bind(this.ChangeLocale).To(vm => vm.ChangeLocaleCommand);
			set.Apply();
		}
	}
}