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

        private Image image;
        public Image Image {
            get { return Image; }
        }


        public double X {
            set { image.SetValue(Canvas.LeftProperty, value); }
        }
        public double Y
        {
            set { image.SetValue(Canvas.TopProperty, value); }
        }
        public Point Point {
            set {
                this.X = value.X;
                this.Y = value.Y;
            }
        }

        public MyImage(string url, BitmapImage bitmapImage)
        {
            this.ulr = url;
            this.bitmapImage = bitmapImage;
        }
    }
}
