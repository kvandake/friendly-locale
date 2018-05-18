namespace FriendlyLocale.Sample.Views
{
    using FriendlyLocale.Sample.Helpers;
    using Xamarin.Forms;

    public class RemoteTranslatePage : BaseTranslatePage
    {
        public RemoteTranslatePage()
        {
            var clearCache = new ToolbarItem
            {
                Icon = "ic_trash",
                Text = "Clear cache",
                Priority = 0,
                Command = new Command(() =>
                {
                    DependencyService.Get<IPersonalFolderHelper>().ClearCache();
                    this.ViewModel?.Reload();
                })
            };

            this.ToolbarItems.Add(clearCache);
        }

        protected override ITranslateViewModel TranslateViewModel => new RemoteTranslateViewModel();
    }
}