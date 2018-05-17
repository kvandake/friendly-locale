namespace FriendlyLocale.Sample.Views
{
    using Xamarin.Forms;

    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page assemblyPage, localePage, remotePage = null;
            var assemblyTranslatePage = new AssemblyTranslatePage();

            assemblyPage = new NavigationPage(assemblyTranslatePage);
            localePage = new NavigationPage(new LocaleTranslatePage());
            remotePage = new NavigationPage(new RemoteTranslatePage());
            assemblyPage.Icon = localePage.Icon = remotePage.Icon = "tab_locale.png";
            assemblyPage.Title = "Assembly";
            localePage.Title = "Local";
            remotePage.Title = "Remote";

            this.Children.Add(assemblyPage);
            this.Children.Add(localePage);
            this.Children.Add(remotePage);

            var firstPage = this.Children[0];
            this.Title = firstPage.Title;
            (assemblyTranslatePage.BindingContext as ITranslateViewModel)?.Reload();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            var currentPage = this.CurrentPage is NavigationPage navigationPage ? navigationPage.CurrentPage : this.CurrentPage;
            if (currentPage?.BindingContext is ITranslateViewModel translateViewModel)
            {
                translateViewModel.Reload();
            }

            this.Title = currentPage?.Title ?? string.Empty;
        }
    }
}