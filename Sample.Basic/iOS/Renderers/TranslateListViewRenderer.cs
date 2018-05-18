using FriendlyLocale.Sample.iOS.Renderers;
using FriendlyLocale.Sample.Views;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(TranslateListView), typeof(TranslateListViewRenderer))]

namespace FriendlyLocale.Sample.iOS.Renderers
{
    using UIKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;

    public class TranslateListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || this.Control == null)
            {
                return;
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                this.Control.CellLayoutMarginsFollowReadableWidth = false;
            }
        }
    }
}