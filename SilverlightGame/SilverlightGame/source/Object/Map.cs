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
using SilverlightGame.Utility;

namespace SilverlightGame.Object
{
    public class Map
    {
        public const int EmptyArea = -1;

        private int rowNum = 60 + 2;
        private int columnNum = 60 + 2;
        public int RowNum {
            get { return rowNum; }
        }
        public int ColumnNum {
            get { return columnNum; }
        }

        public int AreaNum { get; private set; }
        public int GetAreaID(int x, int y)
        {
            return gridDatas[y][x];
        }
        public int GetAreaID(Point<int> position)
        {
            return gridDatas[position.Y][position.X];
        }


        private int verticalMin = 2;
        private int verticalMax = 10;

        private double rightRate = 0.3;
        private int leftMin = 2;
        private int leftMax = 6;
        private int rightMin = 1;
        private int rightMax = 3;

        private GameXXX game;
        private Random random;
        private int[][] gridDatas;

        public Map(GameXXX game)
        {
            this.game = game;
            this.random = game.SyncRandom;

            gridDatas = new int[RowNum][];
            for (int i = 0; i < gridDatas.Length; i++)
            {
                gridDatas[i] = new int[ColumnNum];
            }
        }

        public void Initialize(int seed)
        {
            CreateGridData(seed);
        }

        public void Destroy()
        {
        
        }

        public void CreateGridData(int seed)
        {
            this.random = new Random(seed);
            ClearGridData();

            var newAreaID = 0;
            for (int y = 1; y < gridDatas.Length-1; y++)
            {
                for (int x = 1; x < gridDatas[y].Length-1; x++)
                {
                    if (gridDatas[y][x] == EmptyArea)
                    {
                        FillHorizontal(x, y, ref newAreaID);
                    }
                }
            }
            AreaNum = newAreaID;
        }

        public void ClearGridData()
        {
            for (int y = 0; y < gridDatas.Length; y++)
            {
                for (int x = 0; x < gridDatas[y].Length; x++)
                {
                    gridDatas[y][x] = EmptyArea;
                }
            }
        }

        private void FillHorizontal(int x, int y, ref int newAreaID)
        {
            int length;
            int searchX;
            if (random.NextDouble() < rightRate) {
                length = this.random.Next(rightMax) + rightMin;
                searchX = x - 1;
            }
            else {
                length = this.random.Next(leftMax) + leftMin;
                searchX = x;
            }
            var limit = columnNum - 1;
            length = Math.Min(length, (limit - x));

            var areaID = SearchArea(y, searchX, length);
            bool isNewArea = false;
            if (areaID == EmptyArea)
            {
                areaID = newAreaID;
                newAreaID++;
                isNewArea = true;
            }

            for (int i = 0; i < length; i++)
            {
                if (gridDatas[y][x + i] != EmptyArea) break;

                gridDatas[y][x + i] = areaID;
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
            var limit = RowNum - 1;
            var length = this.random.Next(verticalMax) + verticalMin;
            length = Math.Min(length, (limit - y));

            for (int i = 0; i < length; i++)
            {
                gridDatas[y + i][x] = areaID;
            }
        }

        private int SearchArea(int y, int x, int length)
        {
            for (int i = 0; i < length; i++)
            {
                if (gridDatas[y][x + i] != EmptyArea)
                {
                    return gridDatas[y][x + i];
                }
            }
            return EmptyArea;
        }
    }
}