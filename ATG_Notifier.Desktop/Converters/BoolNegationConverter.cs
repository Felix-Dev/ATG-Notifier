﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ATG_Notifier.Desktop.Converters
{
    internal class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b ? 
                !b 
                : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b ?
                !b
                : false;
        }
    }
}
