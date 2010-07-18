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
using SilverlightGame.Object;

namespace SilverlightGame.source.Object
{
    public class CameraController
    {
        private Camera camera;
        private InputManager input;

        public Point MinLimit;
        public Point MaxLimit;    

        
        public CameraController(Camera camera, InputManager input)
        {
            this.camera = camera;
            this.input = input;
        }

        public void Update(double dt)
        {
            if (input.isMouseLDown())
            {
                var move = input.MouseMove();
                var pos = camera.Position;

                pos.X -= move.X;
                pos.Y -= move.Y;
                pos.X = Math.Max(pos.X, MinLimit.X);
                pos.X = Math.Min(pos.X, MaxLimit.X);
                pos.Y = Math.Max(pos.Y, MinLimit.Y);
                pos.Y = Math.Min(pos.Y, MaxLimit.Y);
                camera.Position = pos;
            }

            var wheelRate = 0.002;
            var maxZoom = 5.0;
            var minZoom = 0.8;
            var zoom = camera.Zoom + input.MouseWheel() * wheelRate;
            zoom = Math.Max(zoom, minZoom);
            zoom = Math.Min(zoom, maxZoom);
            camera.Zoom = zoom;
        }
    }
}
