namespace FriendlyLocale.Sample.Views
{
    using Xamarin.Forms;

    public class ContentDetailsPage : ContentPage
    {
        public ContentDetailsPage()
        {
            var title = new Label
            {
                FontSize = 18,
                LineBreakMode = LineBreakMode.WordWrap
            };

            var singleDescription = new Label
            {
                LineBreakMode = LineBreakMode.WordWrap
            };

            var multiDescription = new Label
            {
                LineBreakMode = LineBreakMode.WordWrap
            };

            var buttonTitle = new Label
            {
                LineBreakMode = LineBreakMode.WordWrap
            };

            var aliasDescription = new Label
            {
                LineBreakMode = LineBreakMode.WordWrap
            };

            this.SetBinding(TitleProperty, "Title");
            title.SetBinding(Label.TextProperty, "TitleLocale");
            singleDescription.SetBinding(Label.TextProperty, "SingleDescription");
            multiDescription.SetBinding(Label.TextProperty, "MultiDescription");
            buttonTitle.SetBinding(Label.TextProperty, "ButtonTitle");
            aliasDescription.SetBinding(Label.TextProperty, "AliasDescription");

            this.Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    title,
                    this.Separator(),
                    singleDescription,
                    this.Separator(),
                    multiDescription,
                    this.Separator(),
                    buttonTitle,
                    this.Separator(),
                    aliasDescription,
                    this.Separator()
                }
            };

            this.ToolbarItems.Add(new ToolbarItem
            {
                Text = "Done",
                Command = new Command(() => this.Navigation.PopModalAsync())
            });

            this.BindingContext = new ContentDetailsViewModel();
        }

        private BoxView Separator()
        {
            return new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = Color.Bisque,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
        }
    }
}