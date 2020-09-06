using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TimVinkemeier.AzureDevOpsToolkit.Converters
{
    public class BooleanInversionConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => !(bool)value;

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}