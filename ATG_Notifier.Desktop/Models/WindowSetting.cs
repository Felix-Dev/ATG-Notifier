using System.Configuration;

namespace ATG_Notifier.Desktop.Models
{
    internal class WindowLocation
    {
        public WindowLocation() {}

        public WindowLocation(double x, double y, double width, double height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public double X { get; set; }

        public double Y { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }
    }
}
