using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ATG_Notifier.Desktop.WPF.Converters
{
    public class CountToVisibilityConverter : IValueConverter
    {
        public Visibility ZeroCountVisibility { get; set; } = Visibility.Collapsed;

        public Visibility PositiveCountVisibility { get; set; } = Visibility.Visible;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count)
            {
                return count > 0
                    ? PositiveCountVisibility
                    : ZeroCountVisibility;
            }

            return ZeroCountVisibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
