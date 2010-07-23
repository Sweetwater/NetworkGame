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
    public class MapDrawer
    {
        private int Width = 640;
        private int Height = 480;

        private GameXXX game;
        private Map map;
        private Random random;

        private Canvas baseCanvas;
        public bool Visible {
            get { return baseCanvas.Visibility == Visibility.Visible; }
            set { baseCanvas.Visibility = (value ? Visibility.Visible : Visibility.Collapsed); }
        }

        private Polygon[] areaShapes;
        public Polygon[] AreaShapes {
            get { return areaShapes; }
        }

        private Point[][] mapGridPoints;
        private PointCollection[] areaPointCollections;

        private Color emptyColor = Color.FromArgb(255,255,255,240);
        private Color strokeColor = Color.FromArgb(255,0,0,0);
        private SolidColorBrush emptyBrush;
        private SolidColorBrush strokeBrush;

        public MapDrawer(GameXXX game)
        {
            this.game = game;
            this.baseCanvas = CreateBaseCanvas();
            this.game.Root.Children.Add(baseCanvas);

            this.emptyBrush = new SolidColorBrush(emptyColor);
            this.strokeBrush = new SolidColorBrush(strokeColor);
        }

        public void Initialize(Map map, int mapSeed)
        {
            this.map = map;
            this.random = new Random(mapSeed);
            CreateShape();
        }

        public void Destroy()
        {
        }

        private Canvas CreateBaseCanvas()
        {
            var baseCanvas = new Canvas();
            var width = game.SreenWidth;
            var height = game.SreenHeight;
            baseCanvas.Width = width;
            baseCanvas.Height = height;
            baseCanvas.RenderTransform = CreateTransform(width, height);

            return baseCanvas;
        }

        private TransformGroup CreateTransform(double baseWidth, double baseHeight)
        {
            var centerTrance = new TranslateTransform();
            centerTrance.X = -(baseWidth / 2.0);
            centerTrance.Y = -(baseHeight / 2.0);

            var transGroup = new TransformGroup();
            transGroup.Children.Add(centerTrance);
            transGroup.Children.Add(game.Camera.TranslateTranform);
            transGroup.Children.Add(game.Camera.ZoomTranform);

            return transGroup;
        }

        private void CreateShape()
        {
            this.mapGridPoints = CreateMapGridPoints();
            this.areaPointCollections = CreateAreaPointCollections();

            var strokeBrush = new SolidColorBrush(strokeColor);
            var emptyBrush = new SolidColorBrush(emptyColor);

            var areaNum = map.AreaNum;
            this.areaShapes = new Polygon[areaNum];

            for (int i = 0; i < areaShapes.Length; i++)
            {
            	var areaShape = CreateAreaShape(i);
                this.areaShapes[i] = areaShape;
                this.baseCanvas.Children.Add(areaShape);
            }
        }

        private Polygon CreateAreaShape(int areaID)
        {
            var doubleCollection = new DoubleCollection();
            doubleCollection.Add(0.5);
            doubleCollection.Add(4.0);

            var areaShape = new Polygon();
            areaShape.Stroke = strokeBrush;
            areaShape.StrokeThickness = 0.5;
            areaShape.StrokeLineJoin = PenLineJoin.Round;
            areaShape.StrokeDashCap = PenLineCap.Square;
            areaShape.StrokeDashArray = doubleCollection;
            areaShape.Fill = emptyBrush;
            areaShape.Points = areaPointCollections[areaID];
            areaShape.Tag = areaID;
            return areaShape;
        }

        private Point[][] CreateMapGridPoints()
        {
            var dataRow = map.RowNum;
            var dataColumn = map.ColumnNum;
            var gridWidth = Width / (double)dataColumn;
            var gridHeight = Height / (double)dataRow;
            var pointSwingH = gridWidth / 2.0 - 0.1;
            var pointSwingV = gridHeight / 2.0 - 0.1;
            
            var mapGridPoints = new Point[dataRow][];

            for (int y = 1; y < mapGridPoints.Length; y++)
            {
                mapGridPoints[y] = new Point[dataColumn];
                for (int x = 1; x < mapGridPoints[y].Length; x++)
                {
                    var swingX = pointSwingH * (random.NextDouble() * 2 - 1);
                    var swingY = pointSwingV * (random.NextDouble() * 2 - 1);
                    var posX = (x - 1) * gridWidth + swingX;
                    var posY = (y - 1) * gridHeight + swingY;
                    mapGridPoints[y][x] = new Point(posX, posY);
                }
            }
            return mapGridPoints;
        }

        private PointCollection[] CreateAreaPointCollections()
        {
            var areaNum = map.AreaNum;
            var dataRow = map.RowNum;
            var dataColumn = map.ColumnNum;
            var areaPointCollections = new PointCollection[areaNum];
            
            for (int y = 1; y < dataRow-1; y++)
            {
                for (int x = 1; x < dataColumn-1; x++)
                {
                    var areaID = map.GetAreaID(x, y);
                    if (areaPointCollections[areaID] == null)
                    {
                        areaPointCollections[areaID] = SearchPoint(y, x);
                    }
                }
            }
            return areaPointCollections;
        }

        private PointCollection SearchPoint(int y, int x)
        {
            // 左手法で探索する
            int left = 0, right = 1, up = 2, down = 3;

            int[] turnLefts = { down, up, left, right };
            int[] turnRights = { up, down, right, left };

            Point<int>[] points = {
                new Point<int>(0,1),
                new Point<int>(1,0),
                new Point<int>(0,0),
                new Point<int>(1,1),
            };
            Point<int>[] hands = {
                new Point<int>(0,1),
                new Point<int>(0,-1),
                new Point<int>(-1,0),
                new Point<int>(1,0),
            };
            Point<int>[] forwards = {
                new Point<int>(-1,0),
                new Point<int>(1,0),
                new Point<int>(0,-1),
                new Point<int>(0,1),
            };

            var area = map.GetAreaID(x,y);
            var dir = right;
            var pos = new Point<int>(x, y);
            var start = pos;

            var collection = new PointCollection();
            collection.Add(mapGridPoints[start.Y][start.X]);

            while (true)
            {
                var point = pos + points[dir];
                var hand = pos + hands[dir];
                var forward = pos + forwards[dir];

                var isHandWall = (area != map.GetAreaID(hand));
                var isForwardWall = (area != map.GetAreaID(forward));

                if (isHandWall)
                {
                    var isEnd = (point == start);
                    if (isEnd) break;

                    collection.Add(mapGridPoints[point.Y][point.X]);

                    if (isForwardWall)
                    {
                        dir = turnRights[dir];
                    }
                }
                else
                {
                    dir = turnLefts[dir];
                }

                if (isHandWall == false || isForwardWall == false)
                {
                    pos += forwards[dir];
                }
            }

            return collection;
        }
    }
}
