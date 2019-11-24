﻿using System.Configuration;

namespace ATG_Notifier.Desktop.Models
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    internal class WindowSetting
    {
        public WindowSetting() {}

        public WindowSetting(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}
