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
using SilverlightGame.source.Utility;

namespace SilverlightGame.Object
{
    public class Camera
    {
        private double moveRate = 1.0;

        private double maxZoom = 5.0;
        private double minZoom = 0.8;
        private double wheelRate = 0.002;

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

        private GameXXX game;

        public Camera(GameXXX game)
        {
            this.game = game;

            this.zoomTranform = new ScaleTransform();
            this.zoomTranform.CenterX = 0;
            this.zoomTranform.CenterY = 0;
            ChangeZoom(1);

            this.translateTranform = new TranslateTransform();
            ChangePosition(new Point(0, 0));
        }

        public void Update(double dt)
        {
            if (game.InputManager.isMouseLDown())
            {
                var move = game.InputManager.MouseMove();
                var pos = this.Position;
                
                pos.X -= move.X * moveRate;
                pos.X = Math.Max(pos.X, -game.CenterX);
                pos.X = Math.Min(pos.X, game.CenterX);

                pos.Y -= move.Y * moveRate;
                pos.Y = Math.Max(pos.Y, -game.CenterY);
                pos.Y = Math.Min(pos.Y, game.CenterY);
                ChangePosition(pos);
            }

            var zoom = this.Zoom + game.InputManager.MouseWheel() * wheelRate;
            zoom = Math.Max(zoom, minZoom);
            zoom = Math.Min(zoom, maxZoom);
            ChangeZoom(zoom);
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
