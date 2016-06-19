using System.Drawing;

namespace WinFormsGameSDK.Drawing
{
    /// <summary>
    /// Represents a start and end point.
    /// </summary>
    public struct Line
    {
        /// <summary>
        /// Gets the start of the line.
        /// </summary>
        public PointF Start { get; }

        /// <summary>
        /// Gets the end of the line.
        /// </summary>
        public PointF End { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="start">The start of the line.</param>
        /// <param name="end">The end of the line.</param>
        public Line(PointF start, PointF end)
        {
            Start = start;
            End = end;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Line"/> class
        /// with the specified arguments.
        /// </summary>
        /// <param name="x1">The start x-position of the line.</param>
        /// <param name="y1">The start y-position of the line.</param>
        /// <param name="x2">The end x-position of the line.</param>
        /// <param name="y2">The end y-position of the line.</param>
        public Line(float x1, float y1, float x2, float y2)
            :this(new PointF(x1, y1), new PointF(x2, y2))
        { }
    }
}
