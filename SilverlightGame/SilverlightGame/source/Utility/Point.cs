using System;

namespace SilverlightGame.Utility
{
    /// <summary>
    /// X座標とY座標の値を持つ構造体です。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct Point<T> : IEquatable<Point<T>> where T : struct
    {
        public T X;
        public T Y;

        /// <summary>
        /// 唯一のコンストラクタです。
        /// </summary>
        /// <param name="x">X座標</param>
        /// <param name="y">Y座標</param>
        public Point(T x, T y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 	ポイントの X 座標と Y 座標を、指定した量だけオフセットします。
        /// </summary>
        /// <param name="offsetX">加算するX座標の量</param>
        /// <param name="offsetY">加算するY座標の量</param>
        public void Offset(T offsetX, T offsetY)
        {
            this.X = this.X.Plus(offsetX);
            this.Y = this.Y.Plus(offsetY);
        }

        /// <summary>
        /// ポイントの X 座標と Y 座標を、指定した量だけオフセットします。
        /// </summary>
        /// <param name="p">加算する量を持ったPoint構造体</param>
        public void Offset(Point<T> p)
        {
            this.X = this.X.Plus(p.X);
            this.Y = this.Y.Plus(p.Y);
        }


        public static Point<T> Add(Point<T> p1, Point<T> p2)
        {
            return new Point<T>(p1.X.Plus(p2.X), p1.Y.Plus(p2.Y));
        }

        public static Point<T> operator +(Point<T> p1, Point<T> p2)
        {
            return Add(p1, p2);
        }

        public static Point<T> Subtract(Point<T> p1, Point<T> p2)
        {
            return new Point<T>(p1.X.Subtract(p2.X), p1.Y.Subtract(p2.Y));
        }

        public static Point<T> operator -(Point<T> p1, Point<T> p2)
        {
            return Subtract(p1, p2);
        }

        /// <summary>
        /// 指定した Object が Point であり、この Point と同じ座標を含んでいるかどうかを判断します。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Point<T>))
            {
                return false;
            }
            return this.Equals((Point<T>)obj);
        }

        /// <summary>
        /// この Point のハッシュ コードを返します。
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }

        public bool Equals(Point<T> other)
        {
            return this.X.Equals(other.X) && this.Y.Equals(other.Y);
        }

        public static bool Equals(Point<T> p1, Point<T> p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator ==(Point<T> p1, Point<T> p2)
        {
            return Equals(p1, p2);
        }

        public static bool operator !=(Point<T> p1, Point<T> p2)
        {
            return !Equals(p1, p2);
        }

        public override string ToString()
        {
            return string.Format(@"X: {0}, Y: {1}", this.X, this.Y);
        }
    }
}
