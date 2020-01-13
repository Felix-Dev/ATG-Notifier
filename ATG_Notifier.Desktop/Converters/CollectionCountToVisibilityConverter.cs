using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ATG_Notifier.Desktop.Converters
{
    internal class CollectionCountToVisibilityConverter : IValueConverter
    {
        public Visibility VisibilityZero { get; set; }

        public Visibility VisibilityNonZero { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int count && count > 0 
                ? VisibilityNonZero
                : VisibilityZero;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
