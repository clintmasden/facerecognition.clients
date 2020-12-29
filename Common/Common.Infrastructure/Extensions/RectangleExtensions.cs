using System;
using System.Drawing;

namespace Common.Infrastructure.Extensions
{
    public static class RectangleExtensions
    {
        //public static Tuple<int, int> GetAreaAndCenterPointChange(System.Drawing.Point centerPoint, Rectangle rectangle)
        //{
        //    var rectangleCenter = rectangle.Center();
        //    var xChange = Math.Abs(centerPoint.X - rectangleCenter.X);
        //    var yChange = Math.Abs(centerPoint.Y - rectangleCenter.Y);

        //    var rectangleCenterPointChange = xChange + yChange;

        //    return new Tuple<int, int>(rectangleArea, rectangleCenterPointChange);
        //}

        public static int CenterDifference(this Rectangle r, Point center)
        {
            var rectangleCenter = r.Center();
            var xChange = Math.Abs(center.X - rectangleCenter.X);
            var yChange = Math.Abs(center.Y - rectangleCenter.Y);

            return xChange + yChange;
        }

        public static int Area(this Rectangle r)
        {
            return r.Width * r.Height;
        }

        public static Point Center(this Rectangle r)
        {
            return new Point((r.Left + r.Right) / 2, (r.Top + r.Bottom) / 2);
        }

        /// <summary>
        ///     Returns the center right point of the rectangle
        ///     i.e. the right hand edge, centered vertically.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center right point of the rectangle</returns>
        public static Point CenterRight(this Rectangle r)
        {
            return new Point(r.Right, (r.Top + r.Bottom) / 2);
        }

        /// <summary>
        ///     Returns the center left point of the rectangle
        ///     i.e. the left hand edge, centered vertically.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center left point of the rectangle</returns>
        public static Point CenterLeft(this Rectangle r)
        {
            return new Point(r.Left, (r.Top + r.Bottom) / 2);
        }

        /// <summary>
        ///     Returns the center bottom point of the rectangle
        ///     i.e. the bottom edge, centered horizontally.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center bottom point of the rectangle</returns>
        public static Point CenterBottom(this Rectangle r)
        {
            return new Point((r.Left + r.Right) / 2, r.Bottom);
        }

        /// <summary>
        ///     Returns the center top point of the rectangle
        ///     i.e. the topedge, centered horizontally.
        /// </summary>
        /// <param name="r"></param>
        /// <returns>Center top point of the rectangle</returns>
        public static Point CenterTop(this Rectangle r)
        {
            return new Point((r.Left + r.Right) / 2, r.Top);
        }
    }
}