using FriendlyLocale.Sample.Droid.Renderers;
using FriendlyLocale.Sample.Views;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(LocaleSwitch), typeof(LocaleSwitchRenderer))]

namespace FriendlyLocale.Sample.Droid.Renderers
{
    using Android.Content;
    using Android.Graphics;
    using Xamarin.Forms.Platform.Android;

    public class LocaleSwitchRenderer : SwitchRenderer
    {
        public LocaleSwitchRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
        {
            base.OnElementChanged(e);

            // do whatever you want to the UISwitch here!
            this.Control?.ThumbDrawable.SetColorFilter(Xamarin.Forms.Color.FromHex("#E60000").ToAndroid(), PorterDuff.Mode.Multiply);
        }
    }
}