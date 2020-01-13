using System;
using System.Globalization;
using System.Windows.Data;

namespace ATG_Notifier.Desktop.Converters
{
    internal class WordCountReleaseTimeToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is int wordCount)
            {
                var releaseTime = values[1] as DateTime?;
                if (wordCount > 0 && releaseTime.HasValue)
                {
                    return $"{wordCount} · {releaseTime.Value.ToString(parameter.ToString())}";
                }
                else if (wordCount > 0)
                {
                    return $"{wordCount}";
                }
                else if (releaseTime.HasValue)
                {
                    return $"{releaseTime.Value.ToString("g")}";
                }
            }

            return "";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
