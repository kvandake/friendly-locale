using MvvmCross.Core.ViewModels;

namespace Sample.MvvmCross.Core.ViewModels
{
	using Acr.UserDialogs;
	using global::MvvmCross.Platform;
	using YamlLocalization;

	public class FirstViewModel : BaseViewModel
	{
		public IMvxCommand ChangeLocaleCommand => new MvxAsyncCommand(async () =>
		{
			var changeLocaleProvider = Mvx.Resolve<IMvxLocalizationProvider>();
			var availableCultures = changeLocaleProvider.GetAvailableCultures();
			var actionSheet = new ActionSheetConfig();
			actionSheet.Title = "Select language culture";
			actionSheet.SetCancel("Cancel");
			foreach (var availableCulture in availableCultures)
			{
				actionSheet.Options.Add(new ActionSheetOption(availableCulture.DisplayName,
					async () =>
					{
						await changeLocaleProvider.ChangeLocale(availableCulture);
						this.RaisePropertyChanged(() => this.LocalizedTextSource);
					}));
			}

			this.UserDialogs.ActionSheet(actionSheet);
		});
	}
}