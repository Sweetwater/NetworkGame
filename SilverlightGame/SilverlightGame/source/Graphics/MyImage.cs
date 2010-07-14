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
using System.Windows.Media.Imaging;

namespace SilverlightGame.Graphics
{
    public class MyImage
    {
        private string ulr;
        public string ULR {
            get { return ulr; }
        }

        private BitmapImage bitmapImage;
        public BitmapImage BitmapImage {
            get { return bitmapImage; }
        }

        public MyImage(string url, BitmapImage bitmapImage)
        {
            this.ulr = url;
            this.bitmapImage = bitmapImage;
        }
    }
}
