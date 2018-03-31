namespace FriendlyLocale.Sample.Views
{
    using FriendlyLocale.Sample.Converters;
    using FriendlyLocale.Sample.Models;
    using Xamarin.Forms;

    public abstract class BaseTranslatePage : ContentPage
    {
        private readonly ListView listView;

        public BaseTranslatePage()
        {
            this.listView = new TranslateListView
            {
                HasUnevenRows = true
            };
            this.Content = this.listView;
            this.listView.ItemTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label
                {
                    FontSize = 14
                };
                var descriptionLabel = new Label
                {
                    FontSize = 12,
                    TextColor = Color.DarkGray,
                    LineBreakMode = LineBreakMode.WordWrap,
                    VerticalOptions = LayoutOptions.FillAndExpand
                };

                nameLabel.SetBinding(Label.TextProperty, "Title");
                descriptionLabel.SetBinding(Label.TextProperty, "Description");
                var checkBox = new LocaleSwitch {IsEnabled = false};
                checkBox.SetBinding(Switch.IsToggledProperty, "IsChecked", BindingMode.TwoWay);
                checkBox.VerticalOptions = LayoutOptions.End;

                // download
                var progressBar = new ProgressBar();
                var downloadLabel = new Label();
                progressBar.SetBinding(ProgressBar.ProgressProperty, "DownloadProgress");
                progressBar.SetBinding(IsVisibleProperty, "IsDownload");
                downloadLabel.SetBinding(IsVisibleProperty, "IsDownload", converter: new InvertedBooleanConverter());
                downloadLabel.SetBinding(Label.TextProperty, "DownloadLabel");
                var downloadView = new StackLayout
                {
                    Children =
                    {
                        progressBar,
                        downloadLabel
                    }
                };
                downloadView.SetBinding(IsVisibleProperty, "IsRemote");

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Padding = new Thickness(0, 5),
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(8, 0, 0, 8),
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new StackLayout
                            {
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                Spacing = 0,
                                Children =
                                {
                                    nameLabel,
                                    descriptionLabel,
                                    downloadView
                                }
                            },
                            checkBox
                        }
                    }
                };
            });

            var translateViewModel = this.TranslateViewModel;
            this.BindingContext = translateViewModel;
            this.Title = translateViewModel.Title;
            this.listView.SetBinding(ListView.ItemsSourceProperty, "Items");
            this.listView.ItemSelected += (s, e) =>
            {
                if (!(e.SelectedItem is LocaleCellElement localeCellElement))
                {
                    return;
                }

                localeCellElement.Command.Execute(localeCellElement);
                this.listView.SelectedItem = null;
            };

            var contentDetails = new ToolbarItem
            {
                Text = "Test Locale",
                Priority = 10,
                Command = new Command(async () =>
                {
                    if (I18N.Instance.CurrentLocale == null)
                    {
                        await this.DisplayAlert("Test Locale", "Please select any locale", "OK");
                        return;
                    }

                    await this.Navigation.PushModalAsync(new NavigationPage(new ContentDetailsPage()));
                })
            };

            this.ToolbarItems.Add(contentDetails);
        }

        public ITranslateViewModel ViewModel => this.BindingContext as ITranslateViewModel;

        protected abstract ITranslateViewModel TranslateViewModel { get; }
    }
}