namespace FriendlyLocale.Sample
{
    public class ContentDetailsViewModel : BaseViewModel
    {
        public ContentDetailsViewModel()
        {
            this.Title = I18N.Instance.Translate("ContentDetailsViewModel.Title");
            this.TitleLocale = I18N.Instance.Translate("ContentDetailsViewModel.TitleLocale");
            this.SingleDescription = I18N.Instance.Translate("ContentDetailsViewModel.SingleDescription");
            this.MultiDescription = I18N.Instance.Translate("ContentDetailsViewModel.MultiDescription");
            this.ButtonTitle = I18N.Instance.Translate("ContentDetailsViewModel.ButtonTitle");
            this.AliasDescription = I18N.Instance.Translate("ContentDetailsViewModel.AliasDescription");
        }

        public string TitleLocale { get; }

        public string SingleDescription { get; }

        public string MultiDescription { get; }

        public string ButtonTitle { get; }

        public string AliasDescription { get; }
    }
}