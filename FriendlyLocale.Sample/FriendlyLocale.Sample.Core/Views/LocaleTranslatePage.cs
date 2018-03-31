namespace FriendlyLocale.Sample.Views
{
    public class LocaleTranslatePage : BaseTranslatePage
    {
        protected override ITranslateViewModel TranslateViewModel => new LocalTranslateViewModel();
    }
}