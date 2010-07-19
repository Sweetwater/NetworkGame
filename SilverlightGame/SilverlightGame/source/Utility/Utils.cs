using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightGame.Utility
{
    public class Utils
    {
        public static Color ToColor(uint color)
        {
            var a = (byte)((color >> 24) & 0x000000FF);
            var r = (byte)((color >> 16) & 0x000000FF);
            var g = (byte)((color >> 8) & 0x000000FF);
            var b = (byte)((color >> 0) & 0x000000FF);
            return Color.FromArgb(a, r, g, b);
        }

        public static Color AddColor(Color color1, Color color2)
        {
            var a = (byte)Math.Min(((int)color1.A + (int)color2.A), 255);
            var r = (byte)Math.Min(((int)color1.R + (int)color2.R), 255);
            var g = (byte)Math.Min(((int)color1.G + (int)color2.G), 255);
            var b = (byte)Math.Min(((int)color1.B + (int)color2.B), 255);
            return Color.FromArgb(a, r, g, b);
        }
    }
}
