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
using TestSilver.Network;

namespace SilverlightGame.Object
{
    public class MapClickController
    {
        public int ClickAreaID { get; private set; }
        public Polygon ClickArea { get; private set; }
        public Point ClickPoint { get; private set; }

        private InputManager input;
        private MapDrawer drawer;

        private double clickRange = 10.0;
        private Polygon downArea;
        private Point downPoint;

        public MapClickController()
        {
        }

        public void Initialize(InputManager inputManager, MapDrawer mapDrawer)
        {
            this.input = inputManager;
            this.drawer = mapDrawer;

            ClickAreaID = Map.EmptyArea;
            ClickArea = null;
            this.downArea = null;

            var areas = drawer.AreaShapes;
            for (int i = 0; i < areas.Length; i++)
            {
                areas[i].MouseLeftButtonUp += AreaMouseButtonUp;
                areas[i].MouseLeftButtonDown += AreaMouseButtonDown;
            }
        }

        public void Destroy()
        {
            var areas = drawer.AreaShapes;
            for (int i = 0; i < areas.Length; i++)
            {
                areas[i].MouseLeftButtonUp -= AreaMouseButtonUp;
                areas[i].MouseLeftButtonDown -= AreaMouseButtonDown;
            }
        }

        public void Update(double dt)
        {
            ClickAreaID = -1;
            ClickArea = null;

            if (input.isMouseLRelease())
            {
                if (this.downArea != null)
                {
                    var releasePoint = input.MousePosition();
                    var diffX = releasePoint.X - downPoint.X;
                    var diffY = releasePoint.Y - downPoint.Y;
                    var length = Math.Sqrt(diffX * diffX + diffY * diffY);

                    if (length < clickRange)
                    {
                        ClickAreaID = (int)downArea.Tag;
                        ClickArea = downArea;
                        ClickPoint = downPoint;
                    }
                }
                this.downArea = null;
            }
        }

        private void AreaMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void AreaMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var downArea = e.OriginalSource as Polygon;
            if (downArea != null)
            {
                this.downArea = downArea;
                this.downPoint = e.GetPosition(null);
            }
        }
    }
}
