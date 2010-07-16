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
using System.Collections.Generic;
using SilverlightGame.source.Utility;

namespace SilverlightGame.Object
{
    public class Map
    {
        private const int emptyArea = -1;
        private const int mapDataWidth = 60;
        private const int mapDataHeight = 60;
        private const int minLength = 2;
        private const int maxLength = 10;
        private const double rightRate = 0.3;
        private const int minLeft = 2;
        private const int maxLeft = 6;
        private const int minRight = 1;
        private const int maxRight = 3;

        private const int mapWidth = 640;
        private const int mapHeight = 480;

        private const double areaWidth = mapWidth / (double)mapDataWidth;
        private const double areaHeight = mapHeight / (double)mapDataHeight;
        private const double areaSwingH = areaWidth / 2d - 0.1d;
        private const double areaSwingV = areaHeight / 2d - 0.1d;

        private int[][] mapData;
        private int areaNum;

        private Canvas mapCanvas;
        private Polygon[] areaShapes;

        private double clickRange = 10.0;
        private Polygon clickArea;
        private Polygon nowClickArea;
        private Polygon buttonDownArea;
        private Point buttonDownPoint;
        public Polygon ClickArea {
            get { return clickArea; }
        }

        public event MouseButtonEventHandler areaMouseButtonUp;
        public event MouseButtonEventHandler areaMouseButtonDown;

        private GameXXX game;
        private Random random;
        
        public Map(GameXXX game)
        {
            this.game = game;
            this.random = game.SyncRandom;

            mapData = new int[mapDataHeight+2][];
            for (int i = 0; i < mapData.Length; i++)
            {
                mapData[i] = new int[mapDataWidth+2];
            }
        }

        public void Initialize()
        {
            this.clickArea = null;
            this.nowClickArea = null;
        }


        public void CreateMap()
        {
            ClearMapData();

            var debugText = "";

            var newAreaID = 0;
            for (int y = 1; y < mapDataHeight+1; y++)
            {
                for (int x = 1; x < mapDataWidth+1; x++)
                {
                    if (mapData[y][x] == emptyArea)
                    {
                        FillHorizontal(x, y, ref newAreaID);
                    }
                    debugText += "" + mapData[y][x];
                }
                debugText += "\n";
            }
        //    debugText += "\n seed : " + seed;
        //    MyLog.WriteTextBlock(debugText);
  
            this.areaNum = newAreaID;

            CreateShape();
        }

        private void FillHorizontal(int x, int y, ref int newAreaID)
        {
            int length;
            int searchX;
            if (random.NextDouble() < rightRate) {
                length = this.random.Next(maxRight) + minRight;
                searchX = x - 1;
            }
            else {
                length = this.random.Next(maxLeft) + minLeft;
                searchX = x;
            }
            length = Math.Min(length, (mapDataWidth + 1 - x));

            var areaID = SearchArea(y, searchX, length);
            bool isNewArea = false;
            if (areaID == emptyArea)
            {
                areaID = newAreaID;
                newAreaID++;
                isNewArea = true;
            }

            for (int i = 0; i < length; i++)
            {
                if (mapData[y][x + i] != emptyArea) break;

                mapData[y][x + i] = areaID;
            }

            if (isNewArea)
            {
                for (int i = 0; i < length; i++)
                {
                    FillVertical(y, x + i, areaID);
                }
            }
        }

        private void FillVertical(int y, int x, int areaID)
        {
            var length = this.random.Next(maxLength) + minLength;
            length = Math.Min(length, (mapDataHeight+1 - y));

            for (int i = 0; i < length; i++)
            {
                mapData[y + i][x] = areaID;
            }
        }

        private int SearchArea(int y, int x, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (mapData[y][x + i] != emptyArea)
                {
                    return mapData[y][x + i];
                }
            }
            return emptyArea;
        }

        private void ClearMapData()
        {
            for (int y = 0; y < mapData.Length; y++)
            {
                for (int x = 0; x < mapData[y].Length; x++)
                {
                    mapData[y][x] = emptyArea;
                }
            }
        }

        private void CreateShape()
        {
            var strokeBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            var fillBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            CreatAreaPoints();

            this.mapCanvas = new Canvas();
            this.mapCanvas.Width = this.game.SreenWidth;
            this.mapCanvas.Height = this.game.SreenHeight;
            this.game.RootContainer.Children.Add(this.mapCanvas);

            var trans = new TranslateTransform();
            trans.X = -(mapCanvas.Width / 2d);
            trans.Y = -(mapCanvas.Height / 2d);

            var transGroup = new TransformGroup();
            transGroup.Children.Add(trans);
            transGroup.Children.Add(this.game.Camera.TranslateTranform);
            transGroup.Children.Add(this.game.Camera.ZoomTranform);
            this.mapCanvas.RenderTransform = transGroup;

            this.areaShapes = new Polygon[areaNum];

            for (int i = 0; i < areaShapes.Length; i++)
            {
                var doubleCollection = new DoubleCollection();
                doubleCollection.Add(1.0);
                doubleCollection.Add(5.0);

                var area = new Polygon();
                area.Stroke = strokeBrush;
                area.StrokeLineJoin = PenLineJoin.Miter;
                area.StrokeDashCap = PenLineCap.Flat;
                area.StrokeDashArray = doubleCollection;
                area.Fill = fillBrush;
                area.Points = areaPoints[i];
                area.Tag = i;
                area.MouseLeftButtonDown += AreaMouseButtonDown;
                area.MouseLeftButtonUp += AreaMouseButtonUp;
                this.mapCanvas.Children.Add(area);
            }
        }


        private PointCollection[] areaPoints;
        private Point[][] allPoints;

        private void CreatAreaPoints()
        {
            CreatAllAreaPoints();

            areaPoints = new PointCollection[areaNum];

            for (int y = 1; y < mapDataHeight+1; y++)
            {
                for (int x = 1; x < mapDataWidth+1; x++)
                {
                    var areaID = mapData[y][x];
                    if (areaPoints[areaID] == null) {
                        areaPoints[areaID] = SearchPoint(y, x);
                    }
                }
            }
        
        }

        private void CreatAllAreaPoints()
        {
            this.allPoints = new Point[mapData.Length][];
            for (int y = 1; y < allPoints.Length; y++)
            {
                this.allPoints[y] = new Point[mapData[y].Length];
                for (int x = 1; x < allPoints[y].Length; x++)
                {
                    var swingX = areaSwingH * (random.NextDouble()* 2 - 1) ;
                    var swingY = areaSwingV * (random.NextDouble()* 2 - 1) ;
                    var posX = (x - 1) * areaWidth + swingX;
                    var posY = (y - 1) * areaHeight + swingY;
                    this.allPoints[y][x] = new Point(posX, posY);
                }
            }
        }

        private PointCollection SearchPoint(int y, int x)
        {
            // 左手法で探索する
            int left = 0, right = 1, up = 2, down = 3;

            int[] turnLefts = { down, up, left, right };
            int[] turnRights = { up, down, right, left };

            int[][] adds = {
                new int[]{0,1},
                new int[]{1,0},
                new int[]{0,0},
                new int[]{1,1},
            };
            int[][] hands = {
                new int[]{0,1},
                new int[]{0,-1},
                new int[]{-1,0},
                new int[]{1,0},
            };
            int[][] forwards = {
                new int[]{-1,0},
                new int[]{1,0},
                new int[]{0,-1},
                new int[]{0,1},
            };

            var area = mapData[y][x];
            var x2 = x;
            var y2 = y;
            var dir = right;
            var add = adds[dir];
            var hand = hands[dir];
            var forward = forwards[dir];

            var startX = x;
            var startY = y;

            var points = new PointCollection();
            points.Add(allPoints[startY][startX]);

            while (true) {
                var oldDir = dir;
                var isHandWall = (area != mapData[y2 + hand[1]][x2 + hand[0]]);
                var isForwardWall = (area != mapData[y2 + forward[1]][x2 + forward[0]]);

                if (isHandWall)
                {
                    var addX = x2 + add[0];
                    var addY = y2 + add[1];

                    var isEnd = (addX == startX && addY == startY);
                    if (isEnd) break;

                    points.Add(allPoints[addY][addX]);

                    if (isForwardWall)
                    {
                        dir = turnRights[dir];
                    }
                }
                else {
                    dir = turnLefts[dir];
                }

                if (oldDir != dir)
                {
                    add = adds[dir];
                    hand = hands[dir];
                    forward = forwards[dir];
                }

                if (isHandWall == false || isForwardWall == false)
                {
                    x2 += forward[0];
                    y2 += forward[1];
                }
            }

            return points;
        }

        private void AreaMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (areaMouseButtonUp != null) {
                this.areaMouseButtonUp(sender, e);
            }
        }
        private void AreaMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (areaMouseButtonDown != null)
            {
                this.areaMouseButtonDown(sender, e);
            }

            var downArea = e.OriginalSource as Polygon;
            if (downArea != null) {
                this.buttonDownArea = downArea;
                this.buttonDownPoint = e.GetPosition(null);
            }
        }

        public void Update(double dt)
        {
            if (game.InputManager.isMouseLRelease())
            {
                if (this.buttonDownArea != null) {
                    var mousePoint = game.InputManager.MousePosition();
                    var diffX = Math.Abs(mousePoint.X - buttonDownPoint.X);
                    var diffY = Math.Abs(mousePoint.Y - buttonDownPoint.Y);
       
                    if (diffX < clickRange && diffY < clickRange)
                    {
                        this.nowClickArea = this.buttonDownArea;
                    }
                }
                this.buttonDownArea = null;
            }

            this.clickArea = this.nowClickArea;
            this.nowClickArea = null;
        }

        public void Draw(double dt)
        {
        }
    }
}