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
using SilverlightGame.Utility;

namespace SilverlightGame.Object
{
    public class Camera
    {
        private double zoom;
        public double Zoom
        {
            get { return zoom; }
            set { ChangeZoom(value); }
        }

        private ScaleTransform zoomTranform;
        public ScaleTransform ZoomTranform {
            get { return zoomTranform; }
        }

        private Point position;
        public Point Position {
            get { return position; }
            set { ChangePosition(value); }
        }

        private Point origin;
        public Point Origin {
            get { return origin; }
            set { ChangeOrigin(value); }
        }

        private TranslateTransform translateTranform;
        public TranslateTransform TranslateTranform
        {
            get { return translateTranform; }
        }

        public Camera()
        {
            this.zoomTranform = new ScaleTransform();
            this.zoomTranform.CenterX = 0;
            this.zoomTranform.CenterY = 0;
            ChangeZoom(1);

            this.translateTranform = new TranslateTransform();
            ChangePosition(new Point(0, 0));
        }

        private void ChangeZoom(double zoom)
        {
            this.zoomTranform.ScaleX = zoom;
            this.zoomTranform.ScaleY = zoom;
            this.zoom = zoom;
        }
        private void ChangePosition(Point position)
        {
            this.translateTranform.X = (origin.X - position.X);
            this.translateTranform.Y = (origin.Y - position.Y);
            this.position = position;
        }

        private void ChangeOrigin(Point origin)
        {
            this.zoomTranform.CenterX = origin.X;
            this.zoomTranform.CenterY = origin.Y;
            this.translateTranform.X = (origin.X - position.X);
            this.translateTranform.Y = (origin.Y - position.Y);
            this.origin = origin;
        }
    }
}
