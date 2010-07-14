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

namespace SilverlightGame.Graphics
{
    public class Graphic
    {
        private GameXXX game;
        private Image canvasImage;

        public Graphic(GameXXX game)
        {
            this.game = game;

            this.canvasImage = new Image();
            game.RootContainer.Children.Add(canvasImage);
        }

        public void DrawImage(MyImage image, double x, double y, int z)
        {
            canvasImage.Source = image.BitmapImage;
            canvasImage.SetValue(Canvas.LeftProperty, x);
            canvasImage.SetValue(Canvas.TopProperty, y);
            canvasImage.SetValue(Canvas.ZIndexProperty, z);
        }
    }
}

//var imageBrush = new ImageBrush
//{
//    Stretch = Stretch.None,
//    AlignmentX = AlignmentX.Left,
//    AlignmentY = AlignmentY.Top
//};
//imageBrush.ImageSource = image;
