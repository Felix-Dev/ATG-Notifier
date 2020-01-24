using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ATG_Notifier.Desktop.Helpers.Extensions
{
    internal static class IconExtensions
    {
        public static ImageSource ToImageSource(this Icon icon)
        {
            if (icon is null)
            {
                throw new ArgumentNullException(nameof(icon));
            }

            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }
    }
}
