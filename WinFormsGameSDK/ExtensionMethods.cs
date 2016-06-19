using System;
using System.Drawing;
using WinFormsGameSDK.Drawing;

namespace WinFormsGameSDK
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Offsets this point by the specified x and y values.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="x">The amount to offset on the x-axis.</param>
        /// <param name="y">The amount to offset on the y-axis.</param>
        /// <returns>The offsetted point.</returns>
        public static PointF Offset(this PointF pos, float x, float y)
        {
            return new PointF(pos.X + x, pos.Y + y);
        }

        /// <summary>
        /// Offsets this point by the specified point.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="point">The x and y values to add to the original point.</param>
        /// <returns>The offsetted point.</returns>
        public static PointF Offset(this PointF pos, PointF point)
        {
            return Offset(pos, point.X, point.Y);
        }

        /// <summary>
        /// Gets the angle, in degrees, between two points.
        /// </summary>
        public static float AngleTo(this PointF p1, PointF p2)
        {
            float xDiff = p1.X - p2.X;
            float yDiff = p1.Y - p2.Y;
            return (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI * -1);
        }

        /// <summary>
        /// Gets the angle, in degrees, between two points.
        /// </summary>
        public static float AngleTo(this Point p1, PointF p2)
        {
            float xDiff = p2.X - p1.X;
            float yDiff = p2.Y - p1.Y;
            return (float)(Math.Atan2(yDiff, xDiff) * 180.0 / Math.PI * -1);
        }

        /// <summary>
        /// Gets the top-left corner of this rectangle.
        /// </summary>
        public static PointF GetTopLeft(this RectangleF rect)
        {
            return new PointF(rect.X, rect.Y);
        }

        /// <summary>
        /// Gets the top-right corner of this rectangle.
        /// </summary>
        public static PointF GetTopRight(this RectangleF rect)
        {
            return new PointF(rect.X + rect.Width, rect.Y);
        }

        /// <summary>
        /// Gets the bottom-right corner of this rectangle.
        /// </summary>
        public static PointF GetBottomRight(this RectangleF rect)
        {
            return new PointF(rect.X + rect.Width, rect.Y + rect.Height);
        }

        /// <summary>
        /// Gets the bottom-left corner of this rectangle.
        /// </summary>
        public static PointF GetBottomLeft(this RectangleF rect)
        {
            return new PointF(rect.X, rect.Y + rect.Height);
        }

        /// <summary>
        /// Draws a rectangle with the dimensions and <see cref="Pen"/> specified.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen">The stroke of the rectangle.</param>
        /// <param name="bounds">The location and size of the rectangle.</param>
        public static void DrawRectangle(this Graphics graphics, Pen pen, RectangleF bounds)
        {
            graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
        }

        /// <summary>
        /// Draws a line with the specified Pen.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen">The stroke to use for the line.</param>
        /// <param name="line">The start and end point.</param>
        public static void DrawLine(this Graphics graphics, Pen pen, Line line)
        {
            graphics.DrawLine(pen, line.Start.X, line.Start.Y, line.End.X, line.End.Y);
        }

        /// <summary>
        /// Draws a line with the specified Pen.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen">The stroke to use for the line.</param>
        /// <param name="lines">The array of lines to draw.</param>
        public static void DrawLines(this Graphics graphics, Pen pen, Line[] lines)
        {
            foreach (var line in lines)
            {
                graphics.DrawLine(pen, line);
            }
        }

        /// <summary>
        /// Gets the center point of this rectangle.
        /// </summary>
        public static PointF GetCenter(this RectangleF rect)
        {
            return new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }

        /// <summary>
        /// Gets the center point of this rectangle.
        /// </summary>
        public static Point GetCenter(this Rectangle rect)
        {
            return new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
        }
    }
}
