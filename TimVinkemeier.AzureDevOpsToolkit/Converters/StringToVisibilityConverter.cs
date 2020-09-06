using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace TimVinkemeier.AzureDevOpsToolkit.Converters
{
    public class StringToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (string)value;
            return string.IsNullOrEmpty(val) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("This converter cannot convert back");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => this;
    }
}