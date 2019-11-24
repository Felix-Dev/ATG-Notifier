using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ATG_Notifier.Desktop.WPF.Converters
{
    internal class NullToBoolConverter : IValueConverter
    {
        public bool NullValue { get; set; }

        public bool NonNullValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is null
                ? NullValue
                : NonNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
