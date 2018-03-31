namespace FriendlyLocale.Sample
{
    using FriendlyLocale.Sample.Views;
    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            {
                this.MainPage = new MainPage();
            }
            else
            {
                this.MainPage = new NavigationPage(new MainPage());
            }
        }
    }
}