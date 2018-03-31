namespace FriendlyLocale.Sample.Converters
{
    using System;
    using System.Globalization;
    using Xamarin.Forms;

    public class InvertedBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }
    }
}