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

namespace SilverlightGame.Utility
{
    public struct IntPoint
    {
        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static bool operator ==(IntPoint point1, IntPoint point2)
        {
            return (point1.X == point2.X && point1.Y == point2.Y);
        }

        public static bool operator !=(IntPoint point1, IntPoint point2)
        {
            return (point1.X != point2.X || point1.Y != point2.Y);
        }

        public static IntPoint operator +(IntPoint point1, IntPoint point2)
        {
            return new IntPoint(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static IntPoint operator +(IntPoint point1, Point point2)
        {
            return new IntPoint(point1.X + (int)point2.X, point1.Y + (int)point2.Y);
        }

        public static IntPoint operator +(Point point1, IntPoint point2)
        {
            return new IntPoint((int)point1.X + point2.X, (int)point1.Y + point2.Y);
        }

        public static IntPoint operator -(IntPoint point1, IntPoint point2)
        {
            return new IntPoint(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static explicit operator Point(IntPoint point)
        {
            return new Point(point.X, point.Y);
        }

        public static explicit operator IntPoint(Point point)
        {
            return new IntPoint((int)point.X, (int)point.Y);
        }

        public override string ToString()
        {
            return "X=" + this.X + ",Y=" + this.Y;
        }
    }
}
