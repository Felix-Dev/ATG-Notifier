using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ATG_Notifier.Desktop.WPF.Helpers.Extensions
{
    internal static class IconExtensions
    {
        internal static ImageSource ToImageSource(this Icon icon)
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
