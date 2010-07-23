using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SilverlightGame.Utility
{
    /// <summary>
    /// Generic operator invokation utility.
    /// </summary>
    /// <example><code>
    /// 123.Plus(456);		// 579
    /// 
    /// // 2009/02/13
    /// (new DateTime(2009, 2, 15)).Addition&lt;DateTime, TimeSpan, DateTime&gt;(new TimeSpan(-2, 0, 0, 0));
    /// </code></example>
    public static class Operators
    {

        #region + (unary)

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class PlusCache<TValue, TResult>
        {
            public static readonly Func<TValue, TResult> Delg = OperatorsUtil.Compile<TValue, TResult>(Expression.UnaryPlus);
        }
        /// <summary>
        /// Same as <c>+(value)</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Plus<T>(this T value)
        {
            return PlusCache<T, T>.Delg(value);
        }
        /// <summary>
        /// Same as <c>+(value)</c>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResult Plus<TValue, TResult>(this TValue value)
        {
            return PlusCache<TValue, TResult>.Delg(value);
        }

        #endregion

        #region + (binary)

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class AdditionCache<TLeft, TRight, TResult>
        {
            public static readonly Func<TLeft, TRight, TResult> Delg = OperatorsUtil.Compile<TLeft, TRight, TResult>(Expression.Add);
        }

        /// <summary>
        /// Same as <c>left + right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static TResult Plus<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return AdditionCache<TLeft, TRight, TResult>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left + right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static TResult Addition<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return AdditionCache<TLeft, TRight, TResult>.Delg(left, right);
        }

        /// <summary>
        /// Same as <c>left + right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Plus<T>(this T left, T right)
        {
            return AdditionCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left + right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Plus<T>(this T left, T? right) where T : struct { return Plus<T?, T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left + right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static T Addition<T>(this T left, T right)
        {
            return AdditionCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left + right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Addition<T>(this T left, T? right) where T : struct { return Addition<T?, T?, T?>(left, right); }

        #endregion

        #region - (unary)

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class NegateCache<TValue, TResult>
        {
            public static readonly Func<TValue, TResult> Delg = OperatorsUtil.Compile<TValue, TResult>(Expression.Negate);
        }

        /// <summary>
        /// Same as <c>-(value)</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Minus<T>(this T value)
        {
            return NegateCache<T, T>.Delg(value);
        }
        /// <summary>
        /// Same as <c>-(value)</c>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResult Minus<TValue, TResult>(this TValue value)
        {
            return NegateCache<TValue, TResult>.Delg(value);
        }

        /// <summary>
        /// Same as <c>-(value)</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Negate<T>(this T value)
        {
            return NegateCache<T, T>.Delg(value);
        }
        /// <summary>
        /// Same as <c>-(value)</c>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static TResult Negate<TValue, TResult>(this TValue value)
        {
            return NegateCache<TValue, TResult>.Delg(value);
        }

        #endregion

        #region - (binary)

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class SubtractCache<TLeft, TRight, TResult>
        {
            public static readonly Func<TLeft, TRight, TResult> Delg = OperatorsUtil.Compile<TLeft, TRight, TResult>(Expression.Subtract);
        }

        /// <summary>
        /// Same as <c>left - right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static TResult Minus<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return SubtractCache<TLeft, TRight, TResult>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left - right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static TResult Subtract<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return SubtractCache<TLeft, TRight, TResult>.Delg(left, right);
        }

        /// <summary>
        /// Same as <c>left - right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Minus<T>(this T left, T right)
        {
            return SubtractCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left - right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Minus<T>(this T left, T? right) where T : struct { return Minus<T?, T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left - right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public static T Subtract<T>(this T left, T right)
        {
            return SubtractCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left - right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Subtract<T>(this T left, T? right) where T : struct { return Subtract<T?, T?, T?>(left, right); }

        #endregion

        #region *

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class MultiplyCache<TLeft, TRight, TResult>
        {
            public static readonly Func<TLeft, TRight, TResult> Delg = OperatorsUtil.Compile<TLeft, TRight, TResult>(Expression.Multiply);
        }

        /// <summary>
        /// Same as <c>left * right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static TResult Multiply<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return MultiplyCache<TLeft, TRight, TResult>.Delg(left, right);
        }

        /// <summary>
        /// Same as <c>left * right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Multiply<T>(this T left, T right)
        {
            return MultiplyCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left * right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Multiply<T>(this T left, T? right) where T : struct { return Multiply<T?, T?, T?>(left, right); }

        #endregion

        #region /

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class DivideCache<TLeft, TRight, TResult>
        {
            public static readonly Func<TLeft, TRight, TResult> Delg = OperatorsUtil.Compile<TLeft, TRight, TResult>(Expression.Divide);
        }

        /// <summary>
        /// Same as <c>left / right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static TResult Divide<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return DivideCache<TLeft, TRight, TResult>.Delg(left, right);
        }

        /// <summary>
        /// Same as <c>left / right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Divide<T>(this T left, T right)
        {
            return DivideCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left / right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Divide<T>(this T left, T? right) where T : struct { return Divide<T?, T?, T?>(left, right); }

        #endregion

        #region %

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        private static class ModuloCache<TLeft, TRight, TResult>
        {
            public static readonly Func<TLeft, TRight, TResult> Delg = OperatorsUtil.Compile<TLeft, TRight, TResult>(Expression.Modulo);
        }

        /// <summary>
        /// Same as <c>left % right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static TResult Modulo<TLeft, TRight, TResult>(this TLeft left, TRight right)
        {
            return ModuloCache<TLeft, TRight, TResult>.Delg(left, right);
        }

        /// <summary>
        /// Same as <c>left % right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T Modulo<T>(this T left, T right)
        {
            return ModuloCache<T, T, T>.Delg(left, right);
        }
        /// <summary>
        /// Same as <c>left % right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static T? Modulo<T>(this T left, T? right) where T : struct { return Modulo<T?, T?, T?>(left, right); }

        #endregion

        #region ==, !=

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        private static class EqualCache<TLeft, TRight>
        {
            public static readonly Func<TLeft, TRight, bool> Equal = OperatorsUtil.Compile<TLeft, TRight, bool>(Expression.Equal);
            public static readonly Func<TLeft, TRight, bool> NotEqual = OperatorsUtil.Compile<TLeft, TRight, bool>(Expression.NotEqual);
        }
        /// <summary>
        /// Same as <c>left == right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equal<TLeft, TRight>(this TLeft left, TRight right)
        {
            return EqualCache<TLeft, TRight>.Equal(left, right);
        }
        /// <summary>
        /// Same as <c>left == right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equal<T>(this T? left, T right) where T : struct { return Equal<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left == right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equal<T>(this T left, T? right) where T : struct { return Equal<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left == right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool Equal<T>(this T? left, T? right) where T : struct { return Equal<T?, T?>(left, right); }


        /// <summary>
        /// Same as <c>left != right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool NotEqual<TLeft, TRight>(this TLeft left, TRight right)
        {
            return EqualCache<TLeft, TRight>.NotEqual(left, right);
        }
        /// <summary>
        /// Same as <c>left != right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool NotEqual<T>(this T? left, T right) where T : struct { return NotEqual<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left != right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool NotEqual<T>(this T left, T? right) where T : struct { return NotEqual<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left != right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool NotEqual<T>(this T? left, T? right) where T : struct { return NotEqual<T?, T?>(left, right); }

        #endregion

        #region <, >

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        private static class CompareOpsCache<TLeft, TRight>
        {
            public static readonly Func<TLeft, TRight, bool> LessThan = OperatorsUtil.Compile<TLeft, TRight, bool>(Expression.LessThan);
            public static readonly Func<TLeft, TRight, bool> GreaterThan = OperatorsUtil.Compile<TLeft, TRight, bool>(Expression.GreaterThan);
        }
        /// <summary>
        /// Same as <c>left &lt; right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LessThan<TLeft, TRight>(this TLeft left, TRight right)
        {
            return CompareOpsCache<TLeft, TRight>.LessThan(left, right);
        }
        /// <summary>
        /// Same as <c>left &lt; right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LessThan<T>(this T? left, T right) where T : struct { return LessThan<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &lt; right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LessThan<T>(this T left, T? right) where T : struct { return LessThan<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &lt; right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LessThan<T>(this T? left, T? right) where T : struct { return LessThan<T?, T?>(left, right); }


        /// <summary>
        /// Same as <c>left &gt; right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GreaterThan<TLeft, TRight>(this TLeft left, TRight right)
        {
            return CompareOpsCache<TLeft, TRight>.GreaterThan(left, right);
        }
        /// <summary>
        /// Same as <c>left &gt; right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GreaterThan<T>(this T? left, T right) where T : struct { return GreaterThan<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &gt; right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GreaterThan<T>(this T left, T? right) where T : struct { return GreaterThan<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &gt; right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GreaterThan<T>(this T? left, T? right) where T : struct { return GreaterThan<T?, T?>(left, right); }

        #endregion

        #region <=, >=

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        private static class CompareOrEqOpsCache<TLeft, TRight>
        {
            public static readonly Func<TLeft, TRight, bool> LessThanOrEqual = OperatorsUtil.Compile<TLeft, TRight, bool>(Expression.LessThanOrEqual);
            public static readonly Func<TLeft, TRight, bool> GreaterThanOrEqual = OperatorsUtil.Compile<TLeft, TRight, bool>(Expression.GreaterThanOrEqual);
        }

        /// <summary>
        /// Same as <c>left &lt;= right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LEq<TLeft, TRight>(this TLeft left, TRight right)
        {
            return CompareOrEqOpsCache<TLeft, TRight>.LessThanOrEqual(left, right);
        }
        /// <summary>
        /// Same as <c>left &lt;= right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LEq<T>(this T? left, T right) where T : struct { return LEq<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &lt;= right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LEq<T>(this T left, T? right) where T : struct { return LEq<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &lt;= right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool LEq<T>(this T? left, T? right) where T : struct { return LEq<T?, T?>(left, right); }

        /// <summary>
        /// Same as <c>left &gt;= right</c>.
        /// </summary>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GEq<TLeft, TRight>(this TLeft left, TRight right)
        {
            return CompareOrEqOpsCache<TLeft, TRight>.GreaterThanOrEqual(left, right);
        }
        /// <summary>
        /// Same as <c>left &gt;= right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GEq<T>(this T? left, T right) where T : struct { return GEq<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &gt;= right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GEq<T>(this T left, T? right) where T : struct { return GEq<T?, T?>(left, right); }
        /// <summary>
        /// Same as <c>left &gt;= right</c>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool GEq<T>(this T? left, T? right) where T : struct { return GEq<T?, T?>(left, right); }

        #endregion

        #region Min, Max

        /// <summary>
        /// Same as: <c>a.LessThan(b) ? a : b</c>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Min<T>(this T a, T b)
        {
            return a.LessThan(b) ? a : b;
        }
        /// <summary>
        /// Get the smallest item of <paramref name="items"/>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// If given list has no item, throws <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Given list has no item.</exception>
        public static T Min<T>(this IEnumerable<T> items)
        {
            T result;
            if (!items.Min(out result)) throw new InvalidOperationException("Given list has no item.");
            return result;
        }
        /// <summary>
        /// Get the smallest item of <paramref name="items"/>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="defaultValue">If given list has no item, retuns this value.</param>
        /// <returns></returns>
        public static T Min<T>(this IEnumerable<T> items, T defaultValue)
        {
            T result;
            return items.Min(out result) ? result : defaultValue;
        }
        /// <summary>
        /// Get the smallest item of <paramref name="items"/>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="result"></param>
        /// <returns>Whether given list isn't empty or is empty.</returns>
        public static bool Min<T>(this IEnumerable<T> items, out T result)
        {
            if (items == null) throw new ArgumentNullException("items");
            result = default(T);

            bool isNotEmpty = false;
            foreach (var item in items)
            {
                if (!isNotEmpty || item.LessThan(result))
                {
                    result = item;
                }
                isNotEmpty = true;
            }
            return isNotEmpty;
        }

        /// <summary>
        /// Same as: <c>a.LessThan(b) ? b : a</c>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static T Max<T>(this T a, T b)
        {
            return a.LessThan(b) ? b : a;
        }
        /// <summary>
        /// Get the smallest item of <paramref name="items"/>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// If given list has no item, throws <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Given list has no item.</exception>
        public static T Max<T>(this IEnumerable<T> items)
        {
            T result;
            if (!items.Max(out result)) throw new InvalidOperationException("Given list has no item.");
            return result;
        }
        /// <summary>
        /// Get the smallest item of <paramref name="items"/>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="defaultValue">If given list has no item, retuns this value.</param>
        /// <returns></returns>
        public static T Max<T>(this IEnumerable<T> items, T defaultValue)
        {
            T result;
            return items.Max(out result) ? result : defaultValue;
        }
        /// <summary>
        /// Get the smallest item of <paramref name="items"/>. 
        /// <typeparamref name="T"/> must have operator &lt;.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="result"></param>
        /// <returns>Whether given list isn't empty or is empty.</returns>
        public static bool Max<T>(this IEnumerable<T> items, out T result)
        {
            if (items == null) throw new ArgumentNullException("items");
            result = default(T);

            bool isNotEmpty = false;
            foreach (var item in items)
            {
                if (!isNotEmpty || result.LessThan(item))
                {
                    result = item;
                }
                isNotEmpty = true;
            }
            return isNotEmpty;
        }

        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static class OperatorsUtil<T>
    {
        /// <summary>
        /// "value" <see cref="ParameterExpression"/>
        /// </summary>
        public static readonly ParameterExpression Value = Expression.Parameter(typeof(T), "value");
        /// <summary>
        /// "left" <see cref="ParameterExpression"/>
        /// </summary>
        public static readonly ParameterExpression Left = Expression.Parameter(typeof(T), "left");
        /// <summary>
        /// "right" <see cref="ParameterExpression"/>
        /// </summary>
        public static readonly ParameterExpression Right = Expression.Parameter(typeof(T), "right");
    }

    /// <summary>
    /// 
    /// </summary>
    public static class OperatorsUtil
    {

        /// <summary>
        /// Make compiled lambda of given <see cref="BinaryExpression"/>. 
        /// Usage: <c>Compile&lt;int&gt;(Expression.Add)  // Returns (int left, int right) =&gt; left + right</c>.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        public static Func<T, T, T> Compile<T>(Func<ParameterExpression, ParameterExpression, BinaryExpression> op)
        {
            return Compile<T, T, T>(op);
        }
        /// <summary>
        /// Make compiled lambda of given <see cref="BinaryExpression"/>.
        /// Usage: <c>Compile&lt;DateTime, TimeSpan, DateTime&gt;(Expression.Add)  // Returns (DateTime left, TimeSpan right) =&gt; left + right</c>.
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <typeparam name="TLeft"></typeparam>
        /// <typeparam name="TRight"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static Func<TLeft, TRight, TResult> Compile<TLeft, TRight, TResult>(Func<ParameterExpression, ParameterExpression, BinaryExpression> op)
        {
            return Expression.Lambda<Func<TLeft, TRight, TResult>>(
                    op(OperatorsUtil<TLeft>.Left, OperatorsUtil<TRight>.Right),
                    OperatorsUtil<TLeft>.Left, OperatorsUtil<TRight>.Right
            ).Compile();
        }

        /// <summary>
        /// Make compiled lambda of given <see cref="UnaryExpression"/>. 
        /// Usage: <c>Compile&lt;int&gt;(Expression.Negate)    // Returns (int x) =&gt; -(x)</c>
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <typeparam name="T"></typeparam>
        public static Func<T, T> Compile<T>(Func<ParameterExpression, UnaryExpression> op)
        {
            return Compile<T, T>(op);
        }
        /// <summary>
        /// Make compiled lambda of given <see cref="UnaryExpression"/>. 
        /// Usage: <c>Compile&lt;int, int&gt;(Expression.Negate)    // Returns (int x) =&gt; -(x)</c>
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public static Func<TValue, TResult> Compile<TValue, TResult>(Func<ParameterExpression, UnaryExpression> op)
        {
            return Expression.Lambda<Func<TValue, TResult>>(
                    op(OperatorsUtil<TValue>.Value),
                    OperatorsUtil<TValue>.Value
            ).Compile();
        }
    }
}