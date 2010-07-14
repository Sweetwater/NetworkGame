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
using SilverlightGame.Graphics;
using System.Collections.Generic;

namespace SilverlightGame.Utility
{
    public static class ResouceManager
    {
        private static Dictionary<string, MyImage> imageDictionary = new Dictionary<string,MyImage>();

        public static MyImage CreateImage(string path) 
        {
            if (imageDictionary.ContainsKey(path)) {
                return imageDictionary[path];
            }

            var bitmapImage = ResourceHelper.GetBitmap(path);
            var myImage = new MyImage(path, bitmapImage);
            return myImage;
        }
    }
}
